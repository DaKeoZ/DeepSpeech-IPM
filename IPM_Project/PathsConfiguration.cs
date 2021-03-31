namespace IPM_Project {
    public class PathsConfiguration {
        
        public string DeepSpeechModelPath { get; set; }
        public string DeepSpeechLMPath { get; set; }
        public string DeepSpeechTriePath { get; set; }
        
        /// <summary>
        /// Constructor
        /// Default values set at the generation of the file
        /// </summary>
        public PathsConfiguration() {
            DeepSpeechModelPath = "..\\..\\Resources\\output_graph.pbmm";
            DeepSpeechLMPath = "..\\..\\Resources\\lm.binary";
            DeepSpeechTriePath = "..\\..\\Resources\\trie";
        }
        
    }
}