namespace MoogleEngine;
using System.Text.RegularExpressions;

public static class Moogle
{
    //esto lo utilizo para el snippet
    public static string[] BestoWordo = new string[tf_idf.reader.textos.Length];
//este metodo es pa convertir mi query en un diccionario...en la linea 32 de este script se ve
    public static Dictionary<string, float> ContarVector(string[] palabras)
    {

        Dictionary<string, float> dic = new Dictionary<string, float>();
        for (int i = 0; i < palabras.Length; i++)
        {
            if (!dic.ContainsKey(palabras[i]))
            {
                dic.Add(palabras[i], 1);
            }
            else
            {
                dic[palabras[i]]++;
            }
        }
        return dic;
    }

    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        List<(SearchItem, int)> list = new List<(SearchItem, int)>(); //la necesite para cuando quiero imprimir en pantalla los mejores 5 textos con respecto a mi query
        string[] palabras = tf_idf.Tokenizar(query);//limpio mi query
        Dictionary<string, float> vectorQuery = ContarVector(palabras);//hago el query un diccionario (cool)
        float sumaRaices = 0; //lo uso en similitud coseno

        //calculo el tf*idf de mi query de la misma manera que lo hice con mis otros diccionarios (por eso hice al query un diccionario)
        Dictionary<string, float>.KeyCollection keys = vectorQuery.Keys;
        foreach (string s in keys)
        {
            if (tf_idf.DiccionarioDeTodosLosTextos.ContainsKey(s))
            {
                vectorQuery[s] = tf_idf.Calculartfidf(vectorQuery[s], palabras.Length, tf_idf.reader.textos.Length, tf_idf.DiccionarioDeTodosLosTextos[s]);

            }
            else
            {
                vectorQuery[s] = 0;

            }
            sumaRaices += vectorQuery[s] * vectorQuery[s];//mas adelante lo uso
            
        }
        //esto de aqui es pa devolver mis mejores 5 documentos en la pantalla
        for( int i = 0; i < tf_idf.reader.textos.Length; i++)
        {                                                      //todo este troncho es poniendo el titulo de cada texto (el +1 es porque se me colaba un / en el nombre de cada titulo   
            SearchItem preDocs = new SearchItem(tf_idf.reader.archivos[i].Substring(Reader.path.Length+1, tf_idf.reader.archivos[i].Length-Reader.path.Length-1), " ", SimilitudCoseno(vectorQuery, i, sumaRaices));
            for( int j = 0; j < 5; j++)
            {
                if(list.Count < 5 )//empiezo a llenar la lista
                {
                    if(preDocs.Score > 0)
                    {
                        list.Insert(j, (preDocs, i));
                        break;
                    }
                }
                else if (list[j].Item1.Score < preDocs.Score)//esto de aqui es para mantener la organizacion de los documentos
                {
                    list.Insert(j, (preDocs, i));
                    break;
                }
            }
        }

        SearchItem[] Docs = new SearchItem[Math.Min(5,list.Count)]; //este array es para quedarme ya con mis 5 mejores Docs organizados

        for( int i = 0; i < Docs.Length; i++)
        {                                                   //mi metodo snippet 
            Docs[i] = new SearchItem(list[i].Item1.Title, Snippet(BestoWordo[list[i].Item2], list[i].Item2, vectorQuery), list[i].Item1.Score);
        }

        string sugerencia = Suggestion(palabras);//implemento mi metodo suggestion

        if (Docs.Length == 0)
        {
            //en caso de no haber ningun resultado poner esto para que no se quede en blanco
            SearchItem a = new SearchItem("No se han encontrado resultados para su búsqueda", " ", 0.11f);
            SearchItem[] noDoc = new SearchItem[1];
            for(int i = 0; i < noDoc.Length; i++)
            {
                noDoc[0] = a;
            }
            return new SearchResult(noDoc, sugerencia);
        } 
        return new SearchResult(Docs, sugerencia);
    }

    //el metodo para tener el score de cada texto con respecto a mi query (es la implementacion de la formula ... en el informe hablo d eso) 
    public static float SimilitudCoseno(Dictionary<string, float> dic, int indice, float sumaRaices)
    {
        float score = 0;
        float maximo = 0;
        float productoEscalar = 0;
        float textoRaices = 0;
        sumaRaices = (float)Math.Sqrt(sumaRaices);
        foreach (var element in dic)
        {
            if (tf_idf.DiccionarioDeTextosEspecificos[indice].ContainsKey(element.Key))
            {
                productoEscalar += element.Value * tf_idf.DiccionarioDeTextosEspecificos[indice][element.Key];

               //estas lineas de aqui es para conseguir mi mejor palabra del query que necesito para el snippet
                if (maximo < tf_idf.DiccionarioDeTextosEspecificos[indice][element.Key])
                {
                    maximo = tf_idf.DiccionarioDeTextosEspecificos[indice][element.Key];
                    BestoWordo[indice] = element.Key;
                }

            }
        }
        foreach (var element in tf_idf.DiccionarioDeTextosEspecificos[indice])
        {
            textoRaices += tf_idf.DiccionarioDeTextosEspecificos[indice][element.Key] * tf_idf.DiccionarioDeTextosEspecificos[indice][element.Key];
        }
        textoRaices = (float)Math.Sqrt(textoRaices);



        //formula de similitud de coseno sumaRaices es la longitud de mi vector query y textoRaices la de mi texto especifico
        score = productoEscalar / (sumaRaices * textoRaices);



        return score;
    }


    //es un snippet rustico pq devuelvo solo el primer trozo donde aparece mi mejor palabra de la query en vez de devolver el mejor trozo
        public static string Snippet(string word, int indice, Dictionary<string, float> vector)
        {
        //aqui el Regex.Replace es un poco diferente al otro que tengo debido a casos esquinados que fui encontrando
            string texto = Regex.Replace(tf_idf.reader.TextosReales[indice], @"[^a-zA-Z0-9áéíóúÁÉÍÓÚäëïöüÄËÏÖÜàèìòùÀÈÌÒÙñÑ]", " ");
            texto = texto.ToLower();
            int textoSize = texto.Length;
            int position = 0;
            
    
        //los lugares donde me puedo encontrar a mi mejor palabra (lo trabaje con char)
            if (texto.IndexOf(word + ' ') == 0)
            {
                position = texto.IndexOf(word);
            }
            else if (texto.IndexOf(' ' + word + ' ') != -1)
            {
                position = texto.IndexOf(' ' + word + ' ');
            }
            else
            {
                position = textoSize - 15;
            }
            //mi rango de char para ambos lados del snippet
            int left = Math.Max(0, position - 100);
        int right = Math.Min(textoSize - 1, position + 100);

        //estos while son para nunca cortar una palabra en el snippet
            while (left != 0 && (textoSize - left < 140 || (texto[left - 1] != ' ' || !char.IsLetterOrDigit(texto[left]))))
            {
                left--;
            }

            while (right != textoSize - 1 && (right < 140 || (texto[right + 1] != ' ' || !char.IsLetterOrDigit(texto[right]))))
            {
                right++;
            }
            //textos reales se creo para este momento (asi puedo devolver mis textos antes de tokenizar, con todos sus simbolos extraños)
            string snippet = tf_idf.reader.TextosReales[indice].Substring(left, right - left + 1);

            return snippet;
        }

    //uno de los metodos mas bonitos de todo el moogle ( se explica en el informe)
    public static int LevenshteinDistance(string pal1, string pal2)
    {
        int costo = 0;
        int a = pal1.Length;
        int b = pal2.Length;
        int[,] m = new int[a + 1, b + 1];

        for (int i = 1; i <= a; m[i, 0] = i++) ;
        for (int i = 1; i <= b; m[0, i] = i++) ;

        for (int i = 1; i <= a; i++)
        {
            for (int j = 1; j <= b; j++)
            {
                costo = (pal1[i - 1] == pal2[j - 1]) ? 0 : 1;
                m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + costo);
            }
        }

        return m[a, b];
    }

    //aqui implemento Levenshtein para mi sugerencia
    public static string Suggestion(string[] vect)
    {
        string suggest = "";
        string PalabraReal = "";

        for (int i = 0; i < vect.Length; i++)
        {
            int costo = int.MaxValue;
            if (tf_idf.DiccionarioDeTodosLosTextos.ContainsKey(vect[i]))
            {
                if (i == vect.Length - 1) suggest += vect[i];
                else suggest += vect[i] + ' ';
            }
            else
            {
                foreach (string word in tf_idf.DiccionarioDeTodosLosTextos.Keys)
                {
                    int temp = LevenshteinDistance(vect[i], word);
                    if (temp < costo)
                    {
                        costo = temp;
                        PalabraReal = word;
                    }

                }
                if (i == vect.Length - 1) suggest += PalabraReal; // me daba TOC el ultimo espacio de la sugerencia e hice esto para quitarlo
                else suggest += PalabraReal + ' ';
            }
        }

        return suggest;
    }

}