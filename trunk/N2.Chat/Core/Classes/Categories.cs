namespace Subgurim.Chat
{
    /// <summary>
    /// Define la Categoria a la que pertenece un canal
    /// </summary>
    public class Categories
    {
        private string _categoria = string.Empty;

        /// <summary>
        /// Nombre de la categor�a. El nombre debe ser �nico
        /// </summary>
        public string categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }

        private string _categoriaMadre = string.Empty;

        /// <summary>
        /// Nombre de la categor�a madre a la que pertenece
        /// </summary>
        public string categor�aMadre
        {
            get { return _categoriaMadre; }
            set { _categoriaMadre = value; }
        }


        public Categories()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}