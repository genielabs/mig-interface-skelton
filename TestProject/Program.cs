using System;

using MIG;
using System.IO;
using MIG.Config;
using System.Xml.Serialization;

namespace TestProject
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine("Mig Interface Skelton test APP");

            var migService = new MigService();

            // Load the configuration from systemconfig.xml file
            MigServiceConfiguration configuration;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer = new XmlSerializer(typeof(MigServiceConfiguration));
            // To read the file, create a FileStream.
            FileStream myFileStream = new FileStream("systemconfig.xml", FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            configuration = (MigServiceConfiguration)mySerializer.Deserialize(myFileStream);

            // Set the configuration and start MIG Service
            migService.Configuration = configuration;
            migService.StartService();

            // Get a reference to the test interface
            var interfaceDomain = "Skelton.InterfaceExample";
            var migInterface = migService.GetInterface(interfaceDomain);
            // Test an interface API command programmatically <module_domain>/<module_address>/<command>[/<option_0>[/../<option_n>]]
            var response = migInterface.InterfaceControl(new MigInterfaceCommand("Skelton.InterfaceExample/3/Greet.Hello/Username"));
            MigService.Log.Debug(response);
            // <module_domain> ::= "Skelton.InterfaceExample"
            // <module_address> ::= "3"
            // <command> ::= "Greet.Hello"
            // <option_0> ::= "Username"
            // For more infos about MIG API see:
            //    http://genielabs.github.io/HomeGenie/api/mig/overview.html
            //    http://genielabs.github.io/HomeGenie/api/mig/mig_api_interfaces.html

            // The same command can be invoked using the WebGateway 
            // http://<server_address>:8080/api/Skelton.InterfaceExample/1/Greet.Hello/Username

            // Test some other interface API command
            response = migInterface.InterfaceControl(new MigInterfaceCommand("Skelton.InterfaceExample/1/Control.On"));
            MigService.Log.Debug(response);
            response = migInterface.InterfaceControl(new MigInterfaceCommand("Skelton.InterfaceExample/1/Control.Off"));
            MigService.Log.Debug(response);
            response = migInterface.InterfaceControl(new MigInterfaceCommand("Skelton.InterfaceExample/2/Temperature.Get"));
            MigService.Log.Debug(response);

            Console.WriteLine("\n[Press Enter to Quit]\n");
            Console.ReadLine();
        }
    }
}
