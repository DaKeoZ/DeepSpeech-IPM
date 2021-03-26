using NUnit.Framework;

namespace IPM_Project
{
    [TestFixture]
    public class CommandInterpretorTests {
        
        private CommandInterpreter CommandInterpreter;
        private string TypicalSentence;
        
        [SetUp]
        public void Setup()
        {
            TypicalSentence = "Bonjour Monsieur";
            CommandInterpreter = new CommandInterpreter(TypicalSentence);
        }

        [Test]
        public void InterpretCommand_OuiGiven_ReturnsYes() {
            
            string voiceCommand = "oui";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.YES.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_ValiderGiven_ReturnsYes() {
            
            string voiceCommand = "valider";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.YES.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_OkGiven_ReturnsYes() {
            
            string voiceCommand = "ok";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.YES.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_SentenceContainingOuiGiven_ReturnsYes() {
            
            string voiceCommand = TypicalSentence + "oui";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.YES.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_SentenceContainingValiderGiven_ReturnsYes() {
            
            string voiceCommand = TypicalSentence + "valider";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.YES.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_SentenceContainingOkGiven_ReturnsYes() {
            
            string voiceCommand = TypicalSentence + "ok";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.YES.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_NonGiven_ReturnsNo() {
            
            string voiceCommand = "non";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.NO.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_RefuserGiven_ReturnsNo() {
            
            string voiceCommand = "refuser";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.NO.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_SentenceContainingNonGiven_ReturnsNo() {
            
            string voiceCommand = TypicalSentence + "non";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.NO.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_SentenceContainingRefuserGiven_ReturnsNo() {
            
            string voiceCommand = TypicalSentence + "refuser";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.NO.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_CommentaireGiven_ReturnsComment() {
            
            string voiceCommand = "commentaire";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.COMMENT.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_SentenceContainingCommentaireGiven_ReturnsComment() {
            
            string voiceCommand = TypicalSentence + "commentaire";
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.COMMENT.ToString(), result.ToString());
        }

        [Test]
        public void InterpretCommand_NoCommandGiven_ReturnsError() {
            
            string voiceCommand = TypicalSentence;
            CommandInterpreter.SetVoiceCommandString(voiceCommand);

            CommandType result = CommandInterpreter.InterpretCommandKeywords();
            
            Assert.AreEqual(CommandType.ERROR.ToString(), result.ToString());
        }
    }
}