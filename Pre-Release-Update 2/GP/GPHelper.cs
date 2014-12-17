//Copyright 2014 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ProSDKSamples.GP {

    internal enum CancelationSource {
        withCancelableProgressor = 0,
        withCancelationToken
    }

    //The DotNet SDK methods for the Geoprocessing module
    internal class GPHelper {

        internal CancellationTokenSource _cts = null;
        private List<KeyValuePair<string, string>> _env = null;
        private static string dataPath = @"C:\Data\SDK\Test\ForTheUC_2014\usa_output.gdb\intrstat_Buffer1"; //change to data on your local drive.

        internal void AddEnvironmentParameter(Tuple<string, string> paramAndValue) {
            if (_env == null)
                _env = new List<KeyValuePair<string, string>>();
            _env.Add(new KeyValuePair<string, string>(paramAndValue.Item1, paramAndValue.Item2));
        }

        internal void ClearEnvironmentParameters() {
            _env = null;
        }
        //Option 1 - use a CancelableProgressor - uses can click on the cancel button when the tool is running
        internal virtual Task<IGPResult> ExecuteToolWithCancelableProgressor(string toolPath, string[] values, CancelableProgressorSource cps) {
            return Geoprocessing.ExecuteTool(toolPath, values, _env.ToArray(), cps.Progressor);
        }
        //Option 2 - use a CancellationTokenSource - you are responsible for implementing your own cancellation. It can
        //either be in the callback or via the 'CallCancel' method implemented in this class
        internal virtual Task<IGPResult> ExecuteToolWithCancellationTokenSource(string toolPath, string[] values) {
            _cts = null;
            _cts = new CancellationTokenSource();
            return Geoprocessing.ExecuteTool(toolPath, values, _env.ToArray(), _cts.Token, (eventName, obj) => {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", eventName, obj.ToString()));
            });
        }
        //Option 3 - open the Geoprocessing tool parameter dialog and interactively execute the tool
        internal virtual void ExecuteToolOpenToolDialog(string toolPath, string[] values) {
            Geoprocessing.OpenToolDialog(toolPath, values, _env.ToArray(), false, null);
        }
        //Call this method to cancel a running GP started with the same CancellationTokenSource
        internal virtual void CallCancel() {
            if (_cts != null) {
                _cts.Cancel();
            }
        }

        internal static async void ExecuteBufferGP(CancelationSource cancelType) {
            
            string toolName = "Buffer_analysis";
            //input layer, output dataset, size of buffer
            string[] toolParams = new string[] {@"intrstat", 
                                       dataPath, 
                                       "30 Miles" };

            //environment parameters from http://resources.arcgis.com/en/help/main/10.2/index.html#//018z0000004s000000
            ProSDKSampleModule.Current.GPHelper.AddEnvironmentParameter(new Tuple<string, string>("OverwriteOutput", "True"));

            IGPResult result = null;
            if (cancelType == CancelationSource.withCancelationToken) {
                result = await ProSDKSampleModule.Current.GPHelper.ExecuteToolWithCancellationTokenSource(toolName, toolParams);
            }
            else {
                CancelableProgressorSource cps = new CancelableProgressorSource(string.Format("{0} running", toolName),
                                                                             string.Format("{0} canceled", toolName));
                result = await ProSDKSampleModule.Current.GPHelper.ExecuteToolWithCancelableProgressor(toolName, toolParams, cps);
            }

            //check the result
            StringBuilder sb = new StringBuilder();
            if (result.IsCanceled) {
                MessageBox.Show(Application.Current.MainWindow, string.Format("{0} was canceled", toolName), toolName);

            }
            else if (result.IsFailed) {
                //there was an error and execution was terminated
                foreach (var msg in result.Messages) {
                    sb.AppendLine(msg.Text);
                }
                string message = string.Format("{0} execution failed\r\n=======================\r\n{1}", toolName, sb.ToString());
                MessageBox.Show(Application.Current.MainWindow, message, toolName + " Error");
            }
            else {
                //success - access results
                foreach (var msg in result.Messages) {
                    sb.AppendLine(msg.Text);
                }
                string message = string.Format("{0} complete\r\n=======================\r\n{1}", toolName, sb.ToString());
                MessageBox.Show(Application.Current.MainWindow, message, toolName + " Success");
            }
        }
    }
}
