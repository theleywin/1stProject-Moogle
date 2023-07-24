using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine;
//este metodo lo hice para leer los TxT
public class Reader

{
    public static string path = Directory.GetCurrentDirectory() + "..\\..\\Content";
    public string[] archivos = Directory.GetFiles(path  , "*.txt", SearchOption.TopDirectoryOnly);
    int cantArchivos;
    public string[] textos { get; private set; }
    public string[] TextosReales { get; private set; }
    public Reader()
    {
        cantArchivos = archivos.Length;
        textos = new string[cantArchivos];
        TextosReales = new string[cantArchivos];

       for (int i = 0; i < cantArchivos; i++)
        {
            textos[i] = File.ReadAllText(archivos[i]).Replace("\r\n", " ");
            TextosReales[i] = File.ReadAllText(archivos[i]);
        }
    }
}
