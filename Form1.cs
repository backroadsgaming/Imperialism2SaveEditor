
using System;
using System.Windows.Forms;
using Imperialism2SaveEditor.Core;
using Imperialism2SaveEditor.Services;

namespace Imperialism2SaveEditor
{
    public partial class Form1 : Form
    {
        private readonly ImperialismSaveFile _saveFile = new();
        private readonly WarehouseReader _warehouseReader = new();

        public Form1()
        {
            InitializeComponent();

            Resources.AutoGenerateColumns = false;
            Resources.Columns.Clear();

            Resources.Columns.Add(
                "ResourceName", "Resource");

            Resources.Columns.Add(
                "Quantity", "Quantity");

            Resources.Columns.Add(
                "Offset", "File Offset");


            Resources.ReadOnly = false;

            // Only the Quantity column can be edited.
            Resources.Columns["ResourceName"]!.ReadOnly = true;
            Resources.Columns["Quantity"]!.ReadOnly = false;
            Resources.Columns["Offset"]!.ReadOnly = true;

            // Prevent users from adding or deleting resource rows.
            Resources.AllowUserToAddRows = false;
            Resources.AllowUserToDeleteRows = false;

        }

        private void btnLoadSave_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Title = "Select Imperialism II Savegame",
                Filter = "Imperialism II Saves (*.imp)|*.imp",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                lblPlayerNation.Text = "Player Nation: Not detected";
                lblTreasury.Text = "Treasury: Not detected";
                Resources.Rows.Clear();

                _saveFile.Load(dialog.FileName);

                NationDetector detector = new NationDetector();

                // Detect the human-controlled nation.
                var playerNation =
                    detector.DetectPlayerNation(_saveFile);

                lblPlayerNation.Text =
                    $"Player Nation: {playerNation.Name}";

                // Detect and read the player's treasury.
                int treasuryOffset =
                    detector.DetectPlayerTreasuryOffset(_saveFile);

                uint treasury =
                    _saveFile.ReadUInt32(treasuryOffset);

                lblTreasury.Text =
                    $"Treasury: ${treasury:N0}";

                // Existing treasury diagnostic report.
                var candidates =
                    detector.FindCandidates(_saveFile);

                string report =
                    $"Treasury candidates found: {candidates.Count}\n\n";

                foreach (var nation in candidates)
                {
                    uint money =
                        _saveFile.ReadUInt32(nation.TreasuryOffset);

                    report +=
                        $"{nation.Name}: " +
                        $"Offset 0x{nation.TreasuryOffset:X} " +
                        $"(${money:N0})\n";
                }

                MessageBox.Show(
                    report,
                    "Nation Detection Diagnostic",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Automatically locate the warehouse.
                int warehouseOffset =
                    _warehouseReader.DetectWarehouseOffset(_saveFile);

                // Read resource quantities at the detected offset.
                var resources =
                    _warehouseReader.ReadResources(
                        _saveFile,
                        warehouseOffset);

                Resources.Rows.Clear();

                foreach (var resource in resources)
                {
                    Resources.Rows.Add(
                        resource.Name,
                        resource.Quantity,
                        $"0x{resource.Offset:X8}");
                }

                MessageBox.Show(
                    $"Loaded {resources.Count} resources successfully.\n\n" +
                    $"Warehouse offset: 0x{warehouseOffset:X8}",
                    "Savegame Loaded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Resources.Rows.Clear();

                MessageBox.Show(
                    ex.Message,
                    "Unable to Read Savegame",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void btnApplyTreasury_Click(object sender, EventArgs e)
        {
            if (!_saveFile.IsLoaded)
            {
                MessageBox.Show("Load a savegame first.");
                return;
            }

            if (!uint.TryParse(txtTreasury.Text.Trim(), out uint newTreasury))
            {
                MessageBox.Show(
                    "Enter a valid treasury amount between 0 and 4,294,967,295.");
                return;
            }

            try
            {
                NationDetector detector = new NationDetector();

                int treasuryOffset =
                    detector.DetectPlayerTreasuryOffset(_saveFile);

                _saveFile.WriteUInt32(treasuryOffset, newTreasury);

                // Save the modified treasury to disk.
                // SaveWriter creates a backup before replacing the original.
                SaveWriter writer = new SaveWriter();
                string backupPath = writer.SaveChanges(_saveFile);

                // Refresh the loaded data and original snapshot after saving.
                _saveFile.Load(_saveFile.FilePath);

                lblTreasury.Text = $"Treasury: ${newTreasury:N0}";

                MessageBox.Show(
                    $"Treasury saved successfully!\n\n" +
                    $"New Treasury: ${newTreasury:N0}\n\n" +
                    $"Original backup:\n{backupPath}",
                    "Treasury Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Treasury Update Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        private void Resources_CellValidating(
            object sender,
            DataGridViewCellValidatingEventArgs e)
        {
            // Validate only the Quantity column.
            if (e.RowIndex < 0 ||
                e.ColumnIndex != Resources.Columns["Quantity"]!.Index)
                return;

            string input = e.FormattedValue?.ToString()?.Trim() ?? "";

            if (!byte.TryParse(input, out _))
            {
                e.Cancel = true;

                MessageBox.Show(
                    "Enter a whole number between 0 and 255.",
                    "Invalid Resource Quantity",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }




        private void btnApplyResources_Click(object sender, EventArgs e)
        {
            if (!_saveFile.IsLoaded)
            {
                MessageBox.Show("Please load a savegame first.");
                return;
            }

            // Commit any quantity currently being edited.
            if (!Resources.EndEdit())
            {
                MessageBox.Show(
                    "Finish editing the resource quantity before applying changes.");
                return;
            }

            // Verify all 23 resource quantities.
            if (Resources.Rows.Count != 23)
            {
                MessageBox.Show(
                    "Expected 23 warehouse resources. Changes were not applied.");
                return;
            }

            foreach (DataGridViewRow row in Resources.Rows)
            {
                string input =
                    row.Cells["Quantity"].Value?.ToString()?.Trim() ?? "";


                if (!byte.TryParse(input, out _))
                {
                    MessageBox.Show(
                        $"Invalid quantity for {row.Cells["ResourceName"].Value}.\n" +
                        "Enter a whole number between 0 and 255.");
                    return;
                }

            }


            try
            {
                // Detect the warehouse again rather than trusting grid offsets.
                int warehouseOffset =
                    _warehouseReader.DetectWarehouseOffset(_saveFile);

                var originalResources =
                    _warehouseReader.ReadResources(_saveFile, warehouseOffset);

                // Verify that every row still matches the detected resource.
                for (int i = 0; i < originalResources.Count; i++)
                {
                    string gridName =
                        Resources.Rows[i].Cells["ResourceName"].Value?.ToString() ?? "";

                    if (gridName != originalResources[i].Name)
                        throw new InvalidOperationException(
                            "Resource order mismatch. Reload the savegame.");
                }

                // All checks passed. Update the in-memory resource values.
                for (int i = 0; i < originalResources.Count; i++)
                {
                    string input =
                        Resources.Rows[i].Cells["Quantity"].Value!.ToString()!;

                    ushort quantity = byte.Parse(input);

                    _saveFile.WriteUInt16(originalResources[i].Offset, quantity);
                }


                // Save all resource modifications to disk.
                // The SaveWriter creates an automatic backup first.
                SaveWriter writer = new SaveWriter();
                string backupPath = writer.SaveChanges(_saveFile);

                // Reload the modified savegame to refresh its original snapshot.
                _saveFile.Load(_saveFile.FilePath);

                // Refresh the warehouse display from the saved file.
                int updatedWarehouseOffset =
                    _warehouseReader.DetectWarehouseOffset(_saveFile);

                var updatedResources =
                    _warehouseReader.ReadResources(_saveFile, updatedWarehouseOffset);

                Resources.Rows.Clear();

                foreach (var resource in updatedResources)
                {
                    Resources.Rows.Add(
                        resource.Name,
                        resource.Quantity,
                        $"0x{resource.Offset:X8}");
                }

                MessageBox.Show(
                    "All 23 resources saved successfully!\n\n" +
                    $"Original backup:\n{backupPath}",
                    "Resources Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Resource Update Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }


    }
}
