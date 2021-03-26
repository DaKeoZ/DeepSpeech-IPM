using System;

namespace IPM_Project
{
    /**
     * Class used to turn a string into a command to send to the REDIS server.
     */
    public class CommandInterpreter {
        
        /**
         * String corresponding to a sentence with a potential keyword understood by deepspeech.
         * Used in InterpretCommandKeywords
         */
        private string VoiceCommandString;
        

        public CommandInterpreter() {
            this.VoiceCommandString = "";
        }
        
        /**
         * Constructor
         * in: Sentence understood by deepspeech
         */
        public CommandInterpreter(string theVoiceCommandString) {
            this.SetVoiceCommandString(theVoiceCommandString);
        }

        /**
         * Interprets voiceCommandString to check for keywords
         * out: String corresponding to a function name related to the first detected keyword or error if none are detected.
         */
        public CommandType InterpretCommandKeywords() {

            foreach (CommandType command in Enum.GetValues(typeof(CommandType))) { // Checks for word in dictionnary
                
                if (VoiceCommandString.Contains(command.ToString())) { // First detected keyword
                    return command;
                }
                
            }
            // No keyword detected 
            return CommandType.ERROR;
        }

        public void SetVoiceCommandString(string aVoiceCommand) {

            this.VoiceCommandString = aVoiceCommand;
        }
    }
}