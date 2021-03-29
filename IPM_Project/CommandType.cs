using System.ComponentModel;

namespace IPM_Project
{
    public enum CommandType {
        /**
         * Default type: Error, used when no key word are detected
         */
        [field:Description("erreur")]
        Error,
        /**
         * Answers yes to a pop up yes/no question
         * keywords: oui, valider, ok
         */
        [field:Description("ok")]
        Yes,
        /**
         * Answers no to a pop up yes/no question
         * keywords: non, refuser
         */
        [field:Description("non")]
        No,
        /**
         * Creates a text area to write down text.
         * keyword: commentaire
         */
        [field:Description("commentaire")]
        Comment,
    }
}