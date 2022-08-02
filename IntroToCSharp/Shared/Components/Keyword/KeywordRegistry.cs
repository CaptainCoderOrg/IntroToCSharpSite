namespace CaptainCoder {
    /// <summary>
    /// The KeywordRegistry acts as a database of Keywords within the site.
    /// You should use the static INSTANCE of this class to access the registry.
    /// It is used in tandem with the Keyword component.
    /// </summary>
    public class KeywordRegistry {
        
        /// <summary>
        /// The singleton INSTANCE of the KeywordRegistry
        /// </summary>
        public static readonly KeywordRegistry INSTANCE = new ();
        private readonly Dictionary<String, String> _registry = new ();

        static KeywordRegistry(){
            INSTANCE._registry["command prompt"] = "The area where you type in your console is called the command prompt.";
            INSTANCE._registry["camel case"] = "Each word is capitalized except for the first (Ex: camelCase)";
            INSTANCE._registry["pascal case"] = "Each word is capitalized (Ex: PascalCase)";
        }

        /// <summary>
        /// Retrieves the definition of the specified keyword. This is case-insensitive
        /// </summary>
        /// <param name="keyword">The keyword to retrieve</param>
        /// <returns>The keyword's definition</returns>
        public string Get(string keyword) => this._registry[keyword.Trim().ToLower()];
        
        /// <summary>
        /// Checks if the specified keyword exists in the registry.
        /// </summary>
        /// <param name="keyword">The keyword to check</param>
        /// <returns>true if the keyword exists and false otherwise</returns>
        public bool Contains(string keyword) => this._registry.ContainsKey(keyword.Trim().ToLower());
        
    }
}