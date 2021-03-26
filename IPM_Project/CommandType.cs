using System.ComponentModel;

namespace IPM_Project
{
    public enum CommandType {
        /**
         * Default type: Error, used when no key word are detected
         */
        [field:Description("erreur")]
        ERROR,
        /**
         * Answers yes to a pop up yes/no question
         * keywords: oui, valider, ok
         */
        [field:Description("ok")]
        YES,
        /**
         * Answers no to a pop up yes/no question
         * keywords: non, refuser
         */
        [field:Description("non")]
        NO,
        /**
         * Creates a text area to write down text.
         * keyword: commentaire
         */
        [field:Description("commentaire")]
        COMMENT,
    }
}