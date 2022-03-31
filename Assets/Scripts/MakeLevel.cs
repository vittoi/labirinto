using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeLevel : MonoBehaviour
{
    public int dim = 21;
    public int tamLabirinto =10;
    public int alturaParede = 30;
    public GameObject bushes;
    public Material materialChao;

    private bool[,] map;
    private GameObject mapa;
    private GameObject floor; 
    private GameObject[,] fisicalMap;
    private GameObject[,] bushesMap;
    private int []where = new int[4];
    


    // Start is called before the first frame update
    void Start()
    {
       
        map = new bool[dim, dim];
        fisicalMap = new GameObject[dim, dim];
        bushesMap  = new GameObject[dim, dim];

        mapa = new GameObject();
        mapa.name = "mapa";
        mapa.transform.position = new Vector3(0, 0, 0);
        
        
        floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.SetParent(mapa.transform);
        floor.name = "chao";
        floor.transform.position = new Vector3(0,0,0);
        floor.transform.localScale = new Vector3(dim * tamLabirinto, 0.1f, dim* tamLabirinto);
        floor.GetComponent<Renderer>().material= materialChao;

        makeLab();

        desenhaLabirinto();
        
    }
    bool verifyMatrix()
    {
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {

                if (map[i, j])
                {
                    bool[] vet = new bool[4];
                    int[] locais = new int[4];
                    vet[0] = i - 1 <= -1 || map[i - 1, j];//cima
                    vet[1] = i + 1 >= dim || map[i + 1, j];//baixo
                    vet[2] = j + 1 >= dim || map[i, j + 1];//direita
                    vet[3] = j - 1 <= -1 || map[i, j - 1];//esquerda
                    int qtdF = 0;
                    for (int t = 0; t < 4; t++)
                    {
                        locais[t] = 0;
                        if (!vet[t])
                        {
                            qtdF++;
                            locais[t] = 1;
                        }
                    }
                    if (qtdF == 3)
                    {

                        if (locais[0] == 1 && locais[1] == 1)
                        {//se locais cima e baixo == 1
                            if ((i + 2 < dim && !map[i + 2, j] && !map[i + 1, j] && i - 1 > -1 && !map[i - 1, j]) || (i - 2 > -1 && !map[i - 2, j] && !map[i - 1, j] && i + 1 < dim && !map[i + 1, j]))
                            {//recomeco a montagem
                                return false;
                            }
                        }
                        else if (locais[2] == 1 && locais[3] == 1)
                        {//se locais esquerda e direita == 1 
                            if ((j + 2 < dim && !map[i, j + 2] && !map[i, j + 1] && j - 1 > -1 && !map[i, j - 1]) || (j - 2 > -1 && !map[i, j - 2] && !map[i, j - 1] && j + 1 < dim && !map[i, j + 1]))
                            {
                                //recomeco a montagem
                                return false;
                            }
                        }
                    }
                }
                //arrayString += string.Format("{0} ", map[i, j]);
            }
        }
        return true;
    }
    void desenhaLabirinto() {
        var flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OffMeshLinkGeneration | StaticEditorFlags.ReflectionProbeStatic;

        GameObjectUtility.SetStaticEditorFlags(mapa, flags);
        GameObjectUtility.SetStaticEditorFlags(floor, flags);

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (map[i, j])
                {
                    desenhaParede(i, j);
                }
            }
        }
        
    }
    void makeLab() {
        int i = 0;
        bool verificado = false;
        while (!verificado )
        {
            mapLab();
            verificado = verifyMatrix();
            i++;
        }
        
    }

    void mapLab() {
        for (int all = 0; all < dim / 2; all++)
        {
            int less = dim - all;

            where[0] = where[1] = all;
            where[2] = where[3] = dim - all - 1;            //print(where[0]+ " "+ where[1] + " " + where[2] + " " + where[3] + " ");
            int colum = (int)Random.Range(0f, 3f);
            Vector2[] openings;
            Vector2 opening;
            Vector2 opening2;

            openings = sortCord(where, colum, all);
            opening = openings[0];
            opening2 = openings[1];

            int stop = 0;

            
            while ((((int)opening.x == where[0] && (int)opening.y == where[3]) || ((int)opening.x == where[3] && (int)opening.y == where[0]) || ((int)opening.x == where[0] && (int)opening.y == where[0]) || ((int)opening.x == where[3] && (int)opening.y == where[3]) ) && stop < 8 )
            {
                openings = sortCord(where, colum, all);
                opening = openings[0];
                opening2 = openings[1];
                stop++;
            }
            

            if ((all % 3 == 0) || all == 0)
            {
                opening2 = new Vector2(-1, -1);
            }
            else if (all == (dim / 2 - 1))
            {
                opening = new Vector2(-1, -1);
            }
            int i = all;
            int j = all;

            for (; i < less; i++)
            {//primeira linha
                if ((i == (int)opening.x && j == (int)opening.y) || (i == (int)opening2.x && j == (int)opening2.y))
                {
                    map[i, j] = !verifySpace(all);
                }
                else
                {
                    map[i, j] = verifySpace(all);
                }
            }
            i--;
            for (; j < less; j++)//ultima coluna
            {
                if ((i == (int)opening.x && j == (int)opening.y) || (i == (int)opening2.x && j == (int)opening2.y))
                {
                    map[i, j] = !verifySpace(all);
                }
                else
                {
                    map[i, j] = verifySpace(all);
                }
            }
            j--;

            for (; i >= all; i--)//ultima linha
            {
                if ((i == (int)opening.x && j == (int)opening.y) || (i == (int)opening2.x && j == (int)opening2.y))
                {
                    map[i, j] = !verifySpace(all);
                }
                else
                {
                    map[i, j] = verifySpace(all);
                }
            }
            i++;

            for (; j >= all; j--)//primeira coluna
            {
                if ((i == (int)opening.x && j == (int)opening.y) || (i == (int)opening2.x && j == (int)opening2.y))
                {
                    map[i, j] = !verifySpace(all);
                }
                else
                {
                    map[i, j] = verifySpace(all);
                }
            }


        }
        

    }


    bool verifySpace(int part) {
        if (part % 2 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Vector2 []sortCord(int[] where, int colum, int passo) {
        Vector2 []opening = new Vector2[2];
        int x, y;
        if (colum % 2 == 0)
        {
            x = where[colum];
            y = (int)Random.Range((float)where[0], (float)where[3]);
            opening[0] = new Vector2(x, y);
            if (colum > 1)
            {
                if (x + 1 < dim && map[(x + 1), y])
                {
                    opening[0].y = (y < where[3] - 1) ? opening[0].y + 1 : opening[0].y - 1;
                    y = (int)opening[0].y;
                }
                opening[1] = new Vector2(where[0], where[3] - (int)y + passo);
            }
            else
            {
                if ( x - 1 >= 0 && map[(x -1), y] )
                {
                    opening[0].y = (y < where[3] - 1) ? opening[0].y + 1 : opening[0].y - 1;
                    y = (int)opening[0].y;
                }
                opening[1] = new Vector2(where[3], where[3] - (int)y + passo);
            }
        }
        else
        {
            x = (int)Random.Range((float)where[0], (float)where[3]);
            y = where[colum];
            opening[0] = new Vector2(x, y);

            if (colum > 1)
            {
                if ((y + 1 < dim) && map[x, (y + 1)])
                {
                    opening[0].x = (x< where[3]-1)? opening[0].x + 1: opening[0].x - 1;
                    x = (int)opening[0].x;
                }
                opening[1] = new Vector2(where[3]- (int)x + passo, where[0]);
            }
            else
            {
                if (y - 1 >= 0 && map[x, (y - 1)]  )
                {
                    opening[0].x = (x < where[3] - 1) ? opening[0].x + 1 : opening[0].x - 1;
                    x = (int)opening[0].x;
                }
                opening[1] = new Vector2(where[3]- (int)x + passo, where[3]);
            }
        }
        
        //print(opening[0].x + " "+ opening[0].y + " " + opening[1].x + " " + opening[1].y);
        return opening;
    }

    void desenhaParede(int i, int j) {
        Vector3 position = new Vector3((float)(i - dim / 2) * tamLabirinto, 0.55f, (float)(j - dim / 2) * tamLabirinto);
        fisicalMap[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fisicalMap[i, j].GetComponent<MeshRenderer>().enabled = false;//TODO isso e pra quando eu decidir oq vai ficar lah mesmo

        //Renderer rend = fisicalMap[i, j].GetComponent<Renderer>();
        //rend.material.color = Color.black;

        var flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OffMeshLinkGeneration | StaticEditorFlags.ReflectionProbeStatic;

        bushesMap[i, j] = Instantiate(bushes);
        GameObjectUtility.SetStaticEditorFlags(bushesMap[i, j], flags);
        bushesMap[i, j].transform.SetParent(fisicalMap[i, j].transform);
        bushesMap[i, j].name = "bush" + i + " " + j;
        bushesMap[i, j].transform.position = new Vector3(0f, 0f, 0f);
        bushesMap[i, j].transform.localScale = new Vector3(1f, 1f, 1f);
        //bushesMap[i, j].transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);//Random.Range(0f, 360f);

        GameObjectUtility.SetStaticEditorFlags(fisicalMap[i, j], flags);
        fisicalMap[i, j].transform.SetParent(mapa.transform);
        fisicalMap[i, j].name = "parede" + i + " " + j;
        fisicalMap[i, j].transform.position = position;
        fisicalMap[i, j].transform.localScale = new Vector3(tamLabirinto, alturaParede, tamLabirinto);
        
        
    }
}
