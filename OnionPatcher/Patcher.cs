using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace OnionPatcher
{
    public partial class Patcher : Form
    {
        public Patcher()
        {
            InitializeComponent();
        }

        private string GetRoot()
        {
            return PathBox.Text;
        }

        private string GetAssemblyDir()
        {
            return PathBox.Text + "/OxygenNotIncluded_Data/Managed";
        }

        private void SetLastPath(FolderBrowserDialog dialog)
        {
            if (File.Exists("data/path.txt"))
            {
                dialog.SelectedPath = File.ReadAllText("data/path.txt");
            }
        }

        private void ValidateBackup()
        {
            RestoreButton.Enabled = false;
            if (File.Exists("data/Assembly-CSharp.dll"))
            {
                RestoreButton.Enabled = true;
            }
        }

        private void ValidateAssembly(string path)
        {
            PatchButton.Enabled = false;
            RestoreButton.Enabled = false;
            // Check if requires assemblies exists
            DefaultAssemblyResolver resolver    = new DefaultAssemblyResolver();
            AssemblyDefinition CSharp           = null;

            resolver.AddSearchDirectory(path);
            try
            {
                CSharp = AssemblyDefinition.ReadAssembly(path + Path.DirectorySeparatorChar + "Assembly-CSharp.dll", new ReaderParameters { AssemblyResolver = resolver });
            }
            catch
            {
                StatusLabel.Text        = "Status: No Assembly in directory.";
                StatusLabel.ForeColor   = Color.Red;
                return;
            }

            ValidateBackup();

            // Check if assemblies are already patched
            TypeDefinition Patched = null;
            try
            {
                Patched = CSharp.Modules
                    .First(M => M.Name == "Assembly-CSharp.dll")
                    .Types
                    .First(T => T.Name == "OnionPatched");
            }
            catch
            {
                Patched = null;
            }

            if (Patched != null)
            {
                StatusLabel.Text        = "Status: Already Patched.";
                StatusLabel.ForeColor   = Color.Red;
                return;
            }
            else
            {
                StatusLabel.Text        = "Status: OK.";
                StatusLabel.ForeColor   = Color.Green;
            }

            
            PatchButton.Enabled = true;
        }

        private void PathBrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                SetLastPath(dialog);
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    File.WriteAllText("data/path.txt", dialog.SelectedPath);
                    PathBox.Text = dialog.SelectedPath;
                    ValidateAssembly(GetAssemblyDir());
                }
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            File.Copy("data/Assembly-CSharp.dll", GetAssemblyDir() + Path.DirectorySeparatorChar + "Assembly-CSharp.dll", true);
            ValidateAssembly(GetAssemblyDir());
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            // Move the necessary files
            File.Copy("data/onion.json", GetRoot() + Path.DirectorySeparatorChar + "onion.json", true);
            File.Copy("data/OnionHooks.dll", GetAssemblyDir() + Path.DirectorySeparatorChar + "OnionHooks.dll", true);
            File.Copy(GetAssemblyDir() + Path.DirectorySeparatorChar + "Assembly-CSharp.dll", "data/Assembly-CSharp.dll", true);
            
            // Patch the Assembly
            // Check if requires assemblies exists
            DefaultAssemblyResolver resolver    = new DefaultAssemblyResolver();
            AssemblyDefinition CSharp           = null;
            AssemblyDefinition Hook             = null;

            resolver.AddSearchDirectory(GetAssemblyDir());
            try
            {
                CSharp = AssemblyDefinition.ReadAssembly(GetAssemblyDir() + Path.DirectorySeparatorChar + "Assembly-CSharp.dll",    new ReaderParameters { AssemblyResolver = resolver });
                Hook  = AssemblyDefinition.ReadAssembly(GetAssemblyDir() + Path.DirectorySeparatorChar + "OnionHooks.dll",         new ReaderParameters { AssemblyResolver = resolver });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            if (CSharp == null || Hook == null)
            {
                return;
            }

            /* Find all injection-points for Assembly-CSharp.dll */
            MethodDefinition InitRandom = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                .Types.First(T => T.Name == "WorldGen")
                .Methods.First(F => F.Name == "InitRandom");


            MethodDefinition OnInitRandom = Hook.Modules.First(M => M.Name == "OnionHooks.dll")
                .Types.First(T => T.Name == "Hooks")
                .Methods.First(M => M.Name == "OnInitRandom");

            MethodDefinition DoWorldGen = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                .Types.First(T => T.Name == "OfflineWorldGen")
                .Methods.First(F => F.Name == "DoWorldGen");

            MethodDefinition DebugHandlerCTOR = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                .Types.First(T => T.Name == "DebugHandler")
                .Methods.First(F => F.Name == ".ctor");

            Console.WriteLine(DebugHandlerCTOR.Name);


            if (InitRandom == null && OnInitRandom == null)
            {
                Console.WriteLine("Missing MethodDefinitions");
                return;
            }

            DoWorldGen.Body.Variables.Add(new VariableDefinition("w", CSharp.MainModule.TypeSystem.Int32));
            DoWorldGen.Body.Variables.Add(new VariableDefinition("h", CSharp.MainModule.TypeSystem.Int32));


            /* Write IL */
            ILProcessor proc = InitRandom.Body.GetILProcessor();
            Instruction IP = InitRandom.Body.Instructions[0];
            try
            {
                
                byte slot_0 = 0;
                byte slot_1 = 1;
                byte slot_2 = 2;
                byte slot_3 = 3;
                byte slot_4 = 4;
                
                // Add hook to Assembly-CSharp.Klei.WorldGen.InitRandom
                proc.InsertBefore(IP, IP = proc.Create(OpCodes.Ldarga_S, slot_1)); // Load the address, since we are passing by ref.
                // Update the compatability
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldarga_S, slot_2));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldarga_S, slot_3));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldarga_S, slot_4));
                proc.InsertAfter // Add call to our dll
                (
                    IP,
                    IP = proc.Create(OpCodes.Call, InitRandom.Module.Import(
                        typeof(OnionHooks.Hooks).GetMethod("OnInitRandom", new[] { typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType() })
                    ))
                );

                // Add hook to <hook>
                DoWorldGen.NoOptimization = true;
                proc    = DoWorldGen.Body.GetILProcessor();
                IP      = DoWorldGen.Body.Instructions[26];

                proc.InsertAfter(proc.Body.Instructions[23], proc.Create(OpCodes.Stloc_2));
                Instruction I = proc.Body.Instructions[26];
                proc.InsertAfter(I, I = proc.Create(OpCodes.Stloc_3));
                proc.InsertAfter(I, I = proc.Create(OpCodes.Ldloca_S, slot_2));
                proc.InsertAfter(I, I = proc.Create(OpCodes.Ldloca_S, slot_3));

                
                proc.InsertBefore(IP, IP = proc.Create(OpCodes.Call, DoWorldGen.Module.Import(
                    typeof(OnionHooks.Hooks).GetMethod("OnDoOfflineWorldGen", new [] {typeof(int).MakeByRefType(), typeof(int).MakeByRefType()})
                    ))
                );

                
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldloc_2));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldloc_3));
                
                FieldReference enabled = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                .Types.First(T => T.Name == "DebugHandler").Fields.First(F => F.Name == "enabled");

                FieldReference camera = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                .Types.First(T => T.Name == "DebugHandler").Fields.First(F => F.Name == "FreeCameraMode");

                Console.WriteLine(camera.Name);
                // Add hook to <hook>
                
                proc = DebugHandlerCTOR.Body.GetILProcessor();
                IP   = DebugHandlerCTOR.Body.Instructions.Last();
                
                
                proc.InsertBefore(IP, IP = proc.Create(OpCodes.Ldarg_0));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Call, DebugHandlerCTOR.Module.Import(
                    typeof(OnionHooks.Hooks).GetMethod("GetDebugEnabled")
                )));
                
                
                //proc.InsertBefore(IP, IP = proc.Create(OpCodes.Ldc_I4_1));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Stfld, enabled));
                

                
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Call, DebugHandlerCTOR.Module.Import(
                    typeof(OnionHooks.Hooks).GetMethod("GetDebugEnabled")
                )));
                

                //proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldc_I4_1));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Stsfld, camera));

                // Hook
                
                MethodDefinition CameraController = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                       .Types.First(T => T.Name == "CameraController")
                       .Methods.First(F => F.Name == ".ctor");

                FieldReference MaxCameraDistance = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                .Types.First(T => T.Name == "CameraController").Fields.First(F => F.Name == "maxOrthographicSize");

                FieldReference MaxCameraDistance2 = CSharp.Modules.First(M => M.Name == "Assembly-CSharp.dll")
                        .Types.First(T => T.Name == "CameraController").Fields.First(F => F.Name == "maxOrthographicSizeDebug");

                proc = CameraController.Body.GetILProcessor();
                IP   = CameraController.Body.Instructions.Last();

                proc.InsertBefore(IP, IP = proc.Create(OpCodes.Ldarg_0));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Call, CameraController.Module.Import(
                    typeof(OnionHooks.Hooks).GetMethod("GetMaxCameraShow")
                )));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Stfld, MaxCameraDistance));

                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Ldarg_0));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Call, CameraController.Module.Import(
                    typeof(OnionHooks.Hooks).GetMethod("GetMaxCameraShow")
                )));
                proc.InsertAfter(IP, IP = proc.Create(OpCodes.Stfld, MaxCameraDistance));
                

                // Add OnionID which is a class that identifies if the Assembly-CSharp.dll has been patched.
                var OnionPatched = new TypeDefinition("", "OnionPatched", TypeAttributes.Class, CSharp.MainModule.TypeSystem.Object);
                CSharp.MainModule.Types.Add(OnionPatched);

                CSharp.Write(GetAssemblyDir() + Path.DirectorySeparatorChar + "Assembly-CSharp.dll");
                MessageBox.Show("Patch Complete!");
                ValidateAssembly(GetAssemblyDir());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
