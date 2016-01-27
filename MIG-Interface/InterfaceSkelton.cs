/*
    This file is part of MIG Project source code.

    MIG is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MIG is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MIG.  If not, see <http://www.gnu.org/licenses/>.  
*/

/*
 *     Author: Generoso Martello <gene@homegenie.it>
 *     Project Homepage: https://github.com/genielabs/mig-service-dotnet
 */

using System;
using System.Collections.Generic;
using System.Linq;

using MIG.Config;

// TODO: notes about ns naming conventions
namespace MIG.Interfaces.Example
{

    public class InterfaceSkelton : MigInterface
    {

        #region MigInterface API commands and events

        public enum Commands
        {
            NotSet,

            Control_On,
            Control_Off,

            Temperature_Get,

            Greet_Hello
        }

        #endregion

        #region Private fields

        private bool isConnected;
        private List<InterfaceModule> modules;

        #endregion

        #region Lifecycle

        public InterfaceSkelton()
        {
            modules = new List<InterfaceModule>();
            // manually add some fake modules
            var module_1 = new InterfaceModule();
            module_1.Domain = this.GetDomain();
            module_1.Address = "1";
            module_1.ModuleType = ModuleTypes.Light;
            var module_2 = new InterfaceModule();
            module_2.Domain = this.GetDomain();
            module_2.Address = "2";
            module_2.ModuleType = ModuleTypes.Sensor;
            var module_3 = new InterfaceModule();
            module_3.Domain = this.GetDomain();
            module_3.Address = "3";
            module_3.ModuleType = ModuleTypes.Sensor;
            // add them to the modules list
            modules.Add(module_1);
            modules.Add(module_2);
            modules.Add(module_3);
        }

        public void Dispose()
        {
            Disconnect();
        }

        #endregion

        #region MIG Interface members

        public event InterfaceModulesChangedEventHandler InterfaceModulesChanged;
        public event InterfacePropertyChangedEventHandler InterfacePropertyChanged;

        public bool IsEnabled { get; set; }

        public List<Option> Options { get; set; }

        public void OnSetOption(Option option)
        {
            if (IsEnabled)
                Connect();
        }

        public List<InterfaceModule> GetModules()
        {
            return modules;
        }

        /// <summary>
        /// Gets a value indicating whether the interface/controller device is connected or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if it is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get { return isConnected; }
        }

        /// <summary>
        /// Returns true if the device has been found in the system
        /// </summary>
        /// <returns></returns>
        public bool IsDevicePresent()
        {
            return true;
        }

        public bool Connect()
        {
            if (!isConnected)
            {
//                OnInterfacePropertyChanged(this.GetDomain(), "IR", "LIRC Remote", "Receiver.RawData", codeparts[ 3 ].TrimEnd(new char[] { '\n', '\r' }) + "/" + codeparts[ 2 ]);
                isConnected = true;
            }
            // TODO...
            OnInterfaceModulesChanged(this.GetDomain());
            return true;
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                // TODO: ...
                isConnected = false;
            }
        }

        public object InterfaceControl(MigInterfaceCommand request)
        {
            var response = new ResponseText("OK"); //default success value

            Commands command;
            Enum.TryParse<Commands>(request.Command.Replace(".", "_"), out command);

            var module = modules.Find (m => m.Address.Equals (request.Address));

            if (module != null) {
                switch (command) {
                case Commands.Control_On:
                // TODO: ...
                    OnInterfacePropertyChanged (this.GetDomain (), request.Address, "Test Interface", "Status.Level", 1);
                    break;
                case Commands.Control_Off:
                    OnInterfacePropertyChanged (this.GetDomain (), request.Address, "Test Interface", "Status.Level", 0);
                // TODO: ...
                    break;
                case Commands.Temperature_Get:
                    OnInterfacePropertyChanged (this.GetDomain (), request.Address, "Test Interface", "Sensor.Temperature", 19.75);
                // TODO: ...
                    break;
                case Commands.Greet_Hello:
                // TODO: ...
                    OnInterfacePropertyChanged (this.GetDomain (), request.Address, "Test Interface", "Sensor.Message", String.Format ("Hello {0}", request.GetOption (0)));
                    response = new ResponseText ("Hello World!");
                    break;
                }
            }
            else 
            {
                response = new ResponseText ("ERROR: invalid module address");
            }

            return response;
        }

        #endregion

        #region Public Members

        // TODO: ....

        #endregion

        #region Private members

        // TODO: ....

        #region Events

        protected virtual void OnInterfaceModulesChanged(string domain)
        {
            if (InterfaceModulesChanged != null)
            {
                var args = new InterfaceModulesChangedEventArgs(domain);
                InterfaceModulesChanged(this, args);
            }
        }

        protected virtual void OnInterfacePropertyChanged(string domain, string source, string description, string propertyPath, object propertyValue)
        {
            if (InterfacePropertyChanged != null)
            {
                var args = new InterfacePropertyChangedEventArgs(domain, source, description, propertyPath, propertyValue);
                InterfacePropertyChanged(this, args);
            }
        }

        #endregion

        #endregion

    }


}

