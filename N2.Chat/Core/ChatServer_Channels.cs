using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace Subgurim.Chat.Server
{
    /// <summary>
    /// La clase Chat es la que maneja el cotarro
    /// </summary>
    public partial class ChatServer
    {
        #region Channels

        #region Insert

        /// <summary>
        /// Adds a channel to the system
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="categoria"></param>
        /// <returns>
        /// 1: correctly added
        /// 0: it already exists!!
        /// -1: error
        /// </returns>
        public static int channel_Add(string channel, string categoria)
        {
            Channel c = new Channel(channel, categoria);

            // Si ya hab�a canales a�adidos (en la cache), tratamos de a�adir el nuevo canal,
            // Comprobando si previamente exist�a uno con el mismo nombre.
            if (myCache.Get(channel_Key_List()) != null)
            {
                if (!channels_Exists(channel))
                {
                    ChatServerBLL.channel_Add(c);
                    ((Dictionary<string, Channel>) (myCache.Get(channel_Key_List()))).Add(c.canal, c);

                    return 1;
                }
                else return 0;
            }
            else
            {
                // si no hab�a canales en la cache, tratamos de leerlos de la base de datos
                Dictionary<string, Channel> canales = ChatServerBLL.channel_List();

                // Y si tampoco hab�a en la fuente de datos, lo creamos nosotros
                if (null == canales)
                    canales = new Dictionary<string, Channel>();

                canales.Add(c.canal, c);

                ChatServerBLL.channel_Add(c);
                myCache.Add(channel_Key_List(), canales, null, DateTime.MaxValue, TimeSpan.Zero,
                            CacheItemPriority.High, null);

                return 1;
            }
        }

        #endregion

        #region List

        /// <summary>
        /// List the channels on the system
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Channel> channels_List()
        {
            object _channels = myCache.Get(channel_Key_List());

            // Si existe en cache, devolvemos el listado
            if (_channels != null)
                return (Dictionary<string, Channel>) _channels;
            else
            {
                // Si no existe en cache, lo recogemos de la fuente de datos y lo a�adimos a la cache
                Dictionary<string, Channel> channels = ChatServerBLL.channel_List();

                if (null != channels)
                    myCache.Add(channel_Key_List(), channels, null, DateTime.MaxValue, TimeSpan.Zero,
                                CacheItemPriority.High, null);

                return channels;
            }
        }

        /// <summary>
        /// List all the channels ordered by categories
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<Channel>> channels_ListByCategories()
        {
            Dictionary<string, List<Channel>> retorno = new Dictionary<string, List<Channel>>();

            // Recorremos todos los canales
            foreach (KeyValuePair<string, Channel> kvp in channels_List())
            {
                // Si a�n no existe esa categor�a
                if (!retorno.ContainsKey(kvp.Value.categoria))
                {
                    // A�adimos al listado de canales de la categor�a
                    List<Channel> canales = new List<Channel>();
                    canales.Add(kvp.Value);
                    retorno.Add(kvp.Value.categoria, canales);
                }
                    // Si la categor�a ya existe
                else
                    // A�adimos el listado de canales de la categor�a.
                    retorno[kvp.Value.categoria].Add(kvp.Value);
            }

            return retorno;
        }

        /// <summary>
        /// List the scheme of the categories
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Channel> channels_CategoriesScheme()
        {
            // De alg�n modo he de devolver el esquema en �rbol de las categor�as.
            // De este modo, junto con channels_ListByCategories(), podr� mostrar al usuario
            // el �rbol con cada categor�a y su hijo.

            return null;
        }

        #endregion

        #region Mantenimiento

        /// <summary>
        /// Deletes a channel from the chat System
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private static int channel_Delete(string channel)
        {
            // Borramos de la fuente de datos (si el canal no existe no nos importa)
            ChatServerBLL.channel_Delete(channel);

            // Si existe en la cache, borraremos
            if (channels_Exists(channel))
                ((Dictionary<string, Channel>) (myCache.Get(channel_Key_List()))).Remove(channel);

            return 1;
        }

        /// <summary>
        /// Do the maintenance for all the channels of the System.
        /// Dado que la petici�n de mantenimiento viene de users, �ste ya tiene un control sobre 
        /// el periodo m�nimo que debe pasar sin mantener (por escalabilidad)
        /// </summary>
        /// <returns></returns>
        private static int channel_maintenance()
        {
            var channels = (Dictionary<string, Channel>) (myCache.Get(channel_Key_List()));

            if (channels != null)
            {
                foreach (KeyValuePair<string, Channel> kvp in channels)
                {
                    channel_maintenance(kvp.Key);
                }
            }

            return 1;
        }

        /// <summary>
        /// Checks if a channel should be deleted
        /// Dado que la petici�n de mantenimiento viene de users, �ste ya tiene un control sobre 
        /// el periodo m�nimo que debe pasar sin mantener (por escalabilidad)
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private static int channel_maintenance(string channel)
        {
            if (myCache.Get(prefijo_canal + channel) != null)
            {
                if (((Dictionary<string, User>) myCache.Get(prefijo_canal + channel)).Count <= 0)
                {
                    channel_Delete(channel);
                    user_DeleteChannel(channel);
                }
            }

            return 1;
        }

        #endregion

        #region Varios

        /// <summary>
        /// Indicates if a channel already exits on the cache
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private static bool channels_Exists(string channel)
        {
            return ((Dictionary<string, Channel>) (myCache.Get(channel_Key_List()))).ContainsKey(channel);
        }

        /// <summary>
        /// Find a channel by name
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static Channel GetChannelByName(string channel)
        {
            foreach (string key in channels_List().Keys)
            {
                if (key.Equals(channel, StringComparison.OrdinalIgnoreCase))
                {
                    Channel c;
                    ((Dictionary<string, Channel>) (myCache.Get(channel_Key_List()))).TryGetValue(key, out c);
                    return c; 
                }
                    
            }
            return null;
        }

        #endregion

        #region Tickets

        /// <summary>
        /// Changes the ticket that indicates when was the channel last changed
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="tick"></param>
        internal static void channel_ChangeWriteTicket(string channel, long tick)
        {
            if (myCache[channel_Key_Token(channel)] == null)
            {
                myCache.Add(channel_Key_Token(channel), tick, null, DateTime.MaxValue, TimeSpan.Zero,
                            CacheItemPriority.Normal, null);
            }
            else
            {
                myCache[channel_Key_Token(channel)] = tick;
            }
        }

        /// <summary>
        /// Access the ticket that indicates when was the channel last changed
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        internal static long channel_AccessWriteTicket(string channel)
        {
            object _tick = myCache.Get(channel_Key_Token(channel));
            if (_tick != null)
                return (long) _tick;
            else
                return long.MaxValue;
        }

        #endregion

        #endregion
    }
}