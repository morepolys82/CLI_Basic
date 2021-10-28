using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CLI_Basic
{
    public partial class CLI_Basic : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        FolderBrowserDialog fbd = new FolderBrowserDialog();

        public CLI_Basic()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            execute();
        }

        private void execute()
        {
            var myList = new List<string>();

            foreach (var files in System.IO.Directory.GetFiles(TxtInputFolder.Text, "*.jpg"))
            {
                /* This goes through the directory and searches for .jpg and will loop these processes */
                /* The config file location is hardcoded for the time being */

                
                // Defining files and paths based on input files
                string fileNoPath = Path.GetFileName(files);
                string fileNoExtension = Path.GetFileNameWithoutExtension(files);
                string fileOutDir = TxtOutputFolder.Text + @"\" + fileNoExtension;

                // Create a directory to output results to
                Directory.CreateDirectory(TxtOutputFolder.Text + @"\" + fileNoExtension);

                /* Setting up config XML file */
                string cliDemoConfig = @"C:\temp\clidemo\cli_video-config.xml";
                String[] masklines = File.ReadAllLines(cliDemoConfig);

                ////////////////////////* Replace the $tokens with the input files */
                for (int i = 1; i <= 49; i++)
                {
                    masklines[i - 1] = masklines[i - 1].Replace("$INPUT", files);
                    masklines[i - 1] = masklines[i - 1].Replace("$OUTPUTDIR", fileOutDir);
                    masklines[i - 1] = masklines[i - 1].Replace("$OUTPUT", fileNoPath);
                }

                File.WriteAllLines(cliDemoConfig, masklines);

                //MessageBox.Show("edited");
                ////////////////////* RUN ARTENGINE HERE */

                string artEngineConfig = "cli_video-config.xml";
                
                //Basepath for config xml
                string configxml = @"C:\temp\clidemo" + artEngineConfig;

                //Params for executable        
                string artEngine = @"--config C:\temp\clidemo\" + artEngineConfig;

                //Execute
                var process = Process.Start(@"C:\Program Files\Artomatix\Artomatix\ArtEngine.CLI\ArtEngine.CLI.exe", artEngine);
                process.WaitForExit(); 
                


                /////////////////////////////* Resetting the config file back to tokens */
                for (int i = 1; i <= 49; i++)
                {
                    masklines[i - 1] = masklines[i - 1].Replace(files, "$INPUT");
                    masklines[i - 1] = masklines[i - 1].Replace(fileOutDir, "$OUTPUTDIR");
                    masklines[i - 1] = masklines[i - 1].Replace(fileNoPath, "$OUTPUT");

                }

                File.WriteAllLines(cliDemoConfig, masklines);
                
            }


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                TxtInputFolder.Text = fbd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                TxtOutputFolder.Text = fbd.SelectedPath;
            }
        }
    }
}
