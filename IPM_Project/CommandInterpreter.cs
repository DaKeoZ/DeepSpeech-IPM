using System;

namespace IPM_Project
{

    /// <summary>
    /// Class used to turn a string into a command to send to the REDIS server.
    /// </summary>
    public class CommandInterpreter {

        /// String corresponding to a sentence with a potential keyword understood by DeepSpeech.
        /// Used in InterpretCommandKeywords
        public string VoiceCommandString;
        
        /// <summary>
        /// Constructor.
        /// Sets the VoiceCommandString to empty if no parameters are passed. 
        /// </summary>
        public CommandInterpreter() {
            this.VoiceCommandString = "";
        }
        
        
        /// <summary>
        /// Constructor.
        /// <param name="theVoiceCommandString">String stored in VoiceCommandString</param>
        /// </summary>
        public CommandInterpreter(string theVoiceCommandString) {
            this.VoiceCommandString = theVoiceCommandString;
        }

        /// <summary>
        /// For every command in the Enum, check if its VoiceCommandString contains one of its keywords
        /// </summary>
        /// <returns>The recognised command if found, the error command if no keywords were found.</returns>
        public CommandType InterpretCommandKeywords() {

            foreach (CommandType command in Enum.GetValues(typeof(CommandType))) { // Checks for word in Enum
                
                if (VoiceCommandString.Contains(command.ToString())) { // First detected keyword
                    return command;
                }
                
            }
            
            // No keyword detected 
            return CommandType.Error;
        }

    }
}