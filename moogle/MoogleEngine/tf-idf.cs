using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoogleEngine
{
    //esta es mi clase tf_idf pero al final termino siendo una mezcla entre calculo de tf*idf y otros metodos de "herramientas"
    public static class tf_idf
    {
        public static Reader reader = new Reader();


        public static Dictionary<string, float>[] DiccionarioDeTextosEspecificos { private set; get; }
        public static Dictionary<string, float> DiccionarioDeTodosLosTextos { private set; get; }


        //este metodo Tokenizar esta estelar pq es el encargado de limpiar las palabras (tokenizarlas)
        //de ahi lo mejor es el Regex.Replace porque me ahorro mucho trabajo (este en especifico solo acepta a los caracteres dentro de las llaves)
        // y los que no estan dentro los convierte en espacio, no los elimina, asi tengo la misma cantidad de char en el texto y no se forman nuevas palabras)
        public static string[] Tokenizar(string s)
        {
            string texto = Regex.Replace(s, @"[^\sa-zA-Z0-9ñÑáéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙ]", " ");
            texto = texto.ToLower();
            return texto.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        //el metodo para calcular el tf*idf de cada palabra(en si estos tres metodos se explican a si solos pq use las variables explicitas para cada cosa
        public static float Calculartfidf(float apariciones, float totalPalabras,
                                   float cantidadTextos, float aparicionTextos)
        {
            
            return TF(apariciones, totalPalabras) * IDF(cantidadTextos, aparicionTextos);
        }


        public static float TF(float apariciones, float totalPalabras)
        {
            return apariciones / totalPalabras;

        }
        //aqui aparece el logaritmo no por arte de magia sino porque asi es la formula del IDF(inverse document frequency)
        public static float IDF(float cantidadTextos, float aparicionTextos)
        {
            float idf = (float)Math.Log10(( cantidadTextos) / ( aparicionTextos));
            return idf;
        }

        //la joya de la corona (el metodo para adquirir cada palabra de cada texto con su valor de tf*idf)
        //lo hice con diccionario por varios motivos, me es comodo trabajar con diccionario, lo considere lo suficientemente optimo y
        //la idea de crear un diccionario para cada texto que tenga sin importar cuantos textos sean esta estelar la verdad
        //pdt: lo hice estatico pq como se entiende que este es el metodo que mas se demora en ejecutarse y solo necesito que lo haga una vez
        //si lo dejaba asi entre busqueda y busqueda iba a volver a ejecutarse, asi q lo volvi static y lo inicialice en la parte visual del Moogle que solo carga una vez
        public static void initiate()
        {

            DiccionarioDeTextosEspecificos = new Dictionary<string, float>[reader.textos.Length];
            DiccionarioDeTodosLosTextos = new Dictionary<string, float>();

            int[] cantPal = new int[reader.textos.Length];

            for (int i = 0; i < reader.textos.Length; i++)
            {
                DiccionarioDeTextosEspecificos[i] = new Dictionary<string, float>();
                string[] palabrasTexto = Tokenizar(reader.textos[i]);
                cantPal[i] = palabrasTexto.Length;

                for (int j = 0; j < palabrasTexto.Length; j++)
                {
                    if (!DiccionarioDeTextosEspecificos[i].ContainsKey(palabrasTexto[j]))//si no esta mi palabra metela y ponle 1 al contador
                    {
                        DiccionarioDeTextosEspecificos[i].Add(palabrasTexto[j], 1);

                        if (!DiccionarioDeTodosLosTextos.ContainsKey(palabrasTexto[j]))//lo mismo que en el de arriba
                        {
                            DiccionarioDeTodosLosTextos.Add(palabrasTexto[j], 1);
                        }
                        else
                        {
                            DiccionarioDeTodosLosTextos[palabrasTexto[j]]++;//esto es pq en el diccionario de todas las palabras nada mas necesito preguntar una sola vez por la palabra, si la vuelvo a encontra salto para la siguiente
                        }

                    }
                    else
                    {
                        DiccionarioDeTextosEspecificos[i][palabrasTexto[j]]++;//aqui si me la vuelvo a encontrar le sumo 1 y sigo
                    }
                }

            }
            //las keys son mis palabras y lo que hago es que si mi texto especifico contiene la palabra que tengo en mi diccionario de todos los textos le calcule su tf*idf
            
            Dictionary<string, float>.KeyCollection keys = DiccionarioDeTodosLosTextos.Keys;
            foreach (string s in keys)
            {
                
                for (int j = 0; j < reader.textos.Length; j++)
                {
                    if (DiccionarioDeTextosEspecificos[j].ContainsKey(s))
                    {
                       
                        DiccionarioDeTextosEspecificos[j][s] = Calculartfidf(DiccionarioDeTextosEspecificos[j][s], cantPal[j],
                                                               reader.textos.Length, DiccionarioDeTodosLosTextos[s]);
                        
                    }
                }
            }
        }
    }
}