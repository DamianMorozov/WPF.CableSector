// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace CableSector.Models
{
    public enum EnumWriteLine { True, False }
    public enum EnumAlgorithm { First, Second, Third }
    public enum EnumSort { Asc, Desc }
    
    /// <summary>
    /// Режим работы.
    /// </summary>
    public enum WorkMode
    {
        Default,
        Canvas,
        Print,
        ChangeLog,
    }

}
