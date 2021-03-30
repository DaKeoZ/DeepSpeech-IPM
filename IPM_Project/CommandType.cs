using System.ComponentModel;

namespace IPM_Project
{
    public enum CommandType {
        
        /// <summary>
        /// Basic error cas.
        /// keyword: Any unrecognized word
        /// </summary>
        [field:Description("erreur")]
        Error,
        /// <summary>
        /// Answers no to a pop-up yes/no question
        /// keyword: "ok"
        /// </summary>
        [field:Description("ok")]
        Yes,
        /// <summary>
        /// Answers no to a pop-up yes/no question
        /// keyword: "non"
        /// </summary>
        [field:Description("non")]
        No,
        /// <summary>
        /// Creates a text area
        /// keyword: "commentaire"
        /// </summary>
        [field:Description("commentaire")]
        Comment,
    }
}