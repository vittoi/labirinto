using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.AI;

public class MakeLevel : MonoBehaviour
{
    public GameObject dataBase;
    public GameObject initPortal;
    public GameObject itemSpot;
    public bool buildWithTrees;
    public bool fogOn = false;
    public int dim = 21;
    public int tamLabirinto =10;
    public int alturaParede = 30;
    public GameObject bushes;
    public Material materialChao;
    public GameObject navMesh;
    public int densityItens; //qtdItens Por Quadrado = tamanho da resta/ density
                             //Ou seja se eu quiser 4 itens no quadrado de 20 entao dou valor da densidade = 5 
                             //Assim tenho 4 itens no quadrado de tamanho 20 e ir dminuindo conforme diminui o tamnaho dos quadrados


    private int[,] map;//0 = chao livre, 1 = parede, 2 = spawn de item
    private GameObject mapa;
    private GameObject floor; 
    private GameObject[,] fisicalMap;
    private GameObject[,] bushesMap;
    private int []where = new int[4];
    private itensList itens;

    private bool firstTime = true;
    


    // Start is called before the first frame update
    void Start()
    {
        shuffleLab();
        firstTime = false;
        
    }

    private void Update()
    {
        if (Manager.Instance.startLab)
        {
            initPortal.SetActive(false);
        }
        
    }

    public void shuffleLab() 
    {
        if (!firstTime)
            Destroy(transform.GetChild(0).gameObject);

        itens = dataBase.GetComponent<itensList>();
        map = new int[dim, dim];
        fisicalMap = new GameObject[dim, dim];
        bushesMap = new GameObject[dim, dim];

        mapa = new GameObject();
        mapa.name = "mapa";
        mapa.transform.parent = GameObject.Find("InitializeGame").transform;
        mapa.transform.localPosition = new Vector3(0, 0, 0);
        mapa.AddComponent<MeshRenderer>();
        mapa.layer = 10;

        #if UNITY_EDITOR
        var flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OffMeshLinkGeneration | StaticEditorFlags.ReflectionProbeStatic;

        GameObjectUtility.SetStaticEditorFlags(mapa, flags);
        #endif

        floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.SetParent(mapa.transform);
        floor.name = "chao";
        floor.transform.position = new Vector3(0, 0.01f, 0);
        floor.transform.localScale = new Vector3(dim * tamLabirinto, 0.1f, dim * tamLabirinto);
        floor.layer = 10;
        floor.GetComponent<MeshRenderer>().enabled = false;

        //floor.GetComponent<Renderer>().material= materialChao;

        makeLab();

        desenhaLabirinto();

        Invoke("bakeNav", 0.5f);

        Manager.Instance.setMap(mapa.transform);
    }

    void bakeNav() {
        NavMeshSurface com = navMesh.GetComponent<NavMeshSurface>();
        com.BuildNavMesh();
       
    }
    bool verifyMatrix()
    {
        for (int i = 0; i < dim; i++)
        {
            int wntb = i%2 == 0? 1: 0;//oq aquele quadrado precisa ser parede ou chao wntb = what need to be
            bool temQ = temQuina(i, wntb);
            if(temQ){//verifica se tem alguma quina como abertura ou parede
                
                return false;
            }
            for (int j = 0; j < dim; j++)
            {

                if (map[i, j] == 1)
                {
                    bool[] vet = new bool[4];
                    int[] locais = new int[4];
                    vet[0] = i - 1 <= -1 || map[i - 1, j] == 1;//cima
                    vet[1] = i + 1 >= dim || map[i + 1, j] == 1;//baixo
                    vet[2] = j + 1 >= dim || map[i, j + 1] == 1;//direita
                    vet[3] = j - 1 <= -1 || map[i, j - 1] == 1;//esquerda
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
                            if ((i + 2 < dim && map[i + 2, j] == 0 && map[i + 1, j] == 0 && i - 1 > -1 && map[i - 1, j] == 0) || (i - 2 > -1 && map[i - 2, j] == 0 && map[i - 1, j] == 0 && i + 1 < dim && map[i + 1, j] == 0))
                            {//recomeco a montagem
                                return false;
                            }
                        }
                        else if (locais[2] == 1 && locais[3] == 1)
                        {//se locais esquerda e direita == 1 
                            if ((j + 2 < dim && map[i, j + 2] == 0 && map[i, j + 1] == 0 && j - 1 > -1 && map[i, j - 1] == 0) || (j - 2 > -1 && map[i, j - 2] == 0 && map[i, j - 1] == 0 && j + 1 < dim && map[i, j + 1] == 0))
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
    
    private bool temQuina(int i, int wntb){
        if(i <= dim/2 && (map[i,i] != wntb || map[i, dim - i- 1] != wntb || map[dim-i-1, i] != wntb || map[dim - i - 1, dim - i - 1] != wntb)){
            return true;
        }
        return false;
    }
   void desenhaLabirinto() {

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (map[i, j] == 1)
                {
                    desenhaParede(i, j);
                } else if (map[i, j] == 3) {
                    desenhaItem(i, j);
                }
            }
        }
        
    }
    void makeLab() {
        bool verificado = false;
        while (!verificado )
        {
            mapLab();
            verificado = verifyMatrix();
        }
        
    }

    void mapLab() {
        //loop na matrix passando por quadrados
        for (int all = 0; all < dim / 2; all++)
        {
            int less = dim - all;

            where[0] = where[1] = all;
            where[2] = where[3] = dim - all - 1;            
            int aresta = (int)Random.Range(0f, 3f); //sorteia em qual aresta do quadrado vai ter a barreira 
            Vector2Int[] sorteados; //vai guardar o valor sorteado e seu espelho
            Vector2Int opening;
            Vector2Int opening2;
            List<Vector2Int> itens = new List<Vector2Int>();
            Vector2Int aux;

            sorteados = sortCord(where, aresta, all);
            opening = sorteados[0];
            opening2 = sorteados[1];

            //No terceiro quadrado so tera uma parede ou abertura
            if ((all % 3 == 0) || all == 0)
            {
                opening2 = new Vector2Int(-1, -1);
            }
            else if (all == (dim / 2 - 1))
            {
                opening = new Vector2Int(-1, -1);

            }
            //Se o quadrado for um de chao
            if (all % 2 == 1)
            {
                //Enquanto nao tiver sorteado os  itens
                while (itens.Count < ((less-all)/densityItens))
                {
                    aresta = (int)Random.Range(0f, 3f);
                    sorteados = sortCord(where, aresta, all);
                    aux = sorteados[0];

                    if (aux != opening && aux != opening2 && !itens.Contains(aux))
                    {                    
                        itens.Add(aux);
                    }
                }
            }

            int i = all;
            int j = all;

            for (; i < less; i++)//aresta superior
            {
                map[i, j] = verifySpace(all);
   
            }
            i--;
            for (; j < less; j++)//aresta da direita
            {
                map[i, j] = verifySpace(all);
               
            }
            j--;

            for (; i >= all; i--)//aresta inferior
            {
                map[i, j] = verifySpace(all);
               
            }
            i++;

            for (; j >= all; j--)//aresta da esquerda
            {
                map[i, j] = verifySpace(all);
                
            }
            //Faz as aberturas, e paredes
            if(opening != new Vector2(-1, -1))
                map[opening.x, opening.y] = map[opening.x, opening.y] == 1 ? 0 : 1;
            if(opening2 != new Vector2(-1, -1))
                map[opening2.x, opening2.y] = map[opening2.x, opening2.y] == 1 ? 0 : 1;

            //passar por toda a minha lista de itens preenchendo cada cordenada com o numero 3
            foreach (var item in itens)
            {
                map[item.x, item.y] = 3;
            }
            

        }


    }

    void desenhaItem(int i, int j) {
        Vector3 position = new Vector3((float)(i - dim / 2) * tamLabirinto, 5f, (float)(j - dim / 2) * tamLabirinto);
        bushesMap[i, j] = Instantiate(itemSpot);

        bushesMap[i, j].transform.SetParent(mapa.transform);
        bushesMap[i, j].name = "item" + i + " " + j;
        bushesMap[i, j].transform.position = position;//new Vector3(0f, 0f, 0f);
        bushesMap[i, j].transform.localScale = new Vector3(1f, 1f, 1f);

        int lastUnlocked = 3;//TODO aqui recebe o indice guardado no player

       GameObject item = Instantiate(itens.getItemByIndex(Random.Range(0, lastUnlocked)));
       setParedesValores(item.GetComponent<ItemEx>().id, bushesMap[i,j].transform);
       item.name = item.name + i+ " "+ j;
       item.transform.position = new Vector3(position.x, position.y -2, position.z);
       item.transform.SetParent(bushesMap[i, j].transform);
       item.GetComponent<ItemEx>().qtd = 10;

    }

    void setParedesValores(int limitSup, Transform itemWalls){
        
        Transform up = itemWalls.GetChild(0);
        Transform down = itemWalls.GetChild(1);
        Transform left = itemWalls.GetChild(2);
        Transform right = itemWalls.GetChild(3);
        
        int idSort = Random.Range(0, limitSup);
        up.GetComponent<paredesFunctions>().valor(idSort);
        
        idSort = Random.Range(0, limitSup);
        down.GetComponent<paredesFunctions>().valor(idSort);
        
        idSort = Random.Range(0, limitSup);
        left.GetComponent<paredesFunctions>().valor(idSort);

        idSort = Random.Range(0, limitSup);
        right.GetComponent<paredesFunctions>().valor(idSort);
    }

    int verifySpace(int part) {
        if (part % 2 == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    Vector2Int []sortCord(int[] where, int colum, int passo) {
        Vector2Int []opening = new Vector2Int[2];
        int x, y;
        if (colum % 2 == 0)
        {
            x = where[colum];
            y = (int)Random.Range((float)where[0], (float)where[3]);
            opening[0] = new Vector2Int(x, y);
            if (colum > 1)
            {
                if (x + 1 < dim && map[(x + 1), y] == 1)
                {
                    opening[0].y = (y < where[3] - 1) ? opening[0].y + 1 : opening[0].y - 1;
                    y = opening[0].y;
                }
                opening[1] = new Vector2Int(where[0], where[3] - (int)y + passo);
            }
            else
            {
                if ( x - 1 >= 0 && map[(x -1), y] == 1)
                {
                    opening[0].y = (y < where[3] - 1) ? opening[0].y + 1 : opening[0].y - 1;
                    y = opening[0].y;
                }
                opening[1] = new Vector2Int(where[3], where[3] - (int)y + passo);
            }
        }
        else
        {
            x = (int)Random.Range((float)where[0], (float)where[3]);
            y = where[colum];
            opening[0] = new Vector2Int(x, y);

            if (colum > 1)
            {
                if ((y + 1 < dim) && map[x, (y + 1)] == 1)
                {
                    opening[0].x = (x< where[3]-1)? opening[0].x + 1: opening[0].x - 1;
                    x = opening[0].x;
                }
                opening[1] = new Vector2Int(where[3]- (int)x + passo, where[0]);
            }
            else
            {
                if (y - 1 >= 0 && map[x, (y - 1)] == 1)
                {
                    opening[0].x = (x < where[3] - 1) ? opening[0].x + 1 : opening[0].x - 1;
                    x = opening[0].x;
                }
                opening[1] = new Vector2Int(where[3]- (int)x + passo, where[3]);
            }
        }
        
        //print(opening[0].x + " "+ opening[0].y + " " + opening[1].x + " " + opening[1].y);
        return opening;
    }

    void desenhaParede(int i, int j) {
        Vector3 position = new Vector3((float)(i - dim / 2) * tamLabirinto, 0.55f, (float)(j - dim / 2) * tamLabirinto);
        fisicalMap[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fisicalMap[i, j].layer = 7;
        if (buildWithTrees)
        {
            fisicalMap[i, j].GetComponent<MeshRenderer>().enabled = false;

            bushesMap[i, j] = Instantiate(bushes);
            bushesMap[i, j].transform.SetParent(mapa.transform);
            bushesMap[i, j].transform.position = position;
            bushesMap[i, j].name = "bush" + i + " " + j;
            bushesMap[i, j].layer = 10;

            Transform tronco = bushesMap[i, j].transform.GetChild(0);
            if (Random.Range(0, 10) > 2)
            {
                tronco.gameObject.SetActive(false);
            }
            bushesMap[i, j].transform.position = new Vector3(position.x, position.y - 0.5f, position.z);
            bushesMap[i, j].transform.localScale = new Vector3(10f, 10f, 10f);
            bushesMap[i, j].transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);//Random.Range(0f, 360f);

            #if UNITY_EDITOR
                var flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OffMeshLinkGeneration | StaticEditorFlags.ReflectionProbeStatic;

                GameObjectUtility.SetStaticEditorFlags(bushesMap[i, j], flags);
            #endif
        }

        fisicalMap[i, j].transform.SetParent(mapa.transform);
        fisicalMap[i, j].name = "parede" + i + " " + j;
        fisicalMap[i, j].transform.position =new Vector3(position.x, position.y +50f, position.z);;
        fisicalMap[i, j].transform.localScale = new Vector3(tamLabirinto, alturaParede, tamLabirinto);
        fisicalMap[i, j].GetComponent<MeshRenderer>().material.color = Color.black;

        fisicalMap[i, j].AddComponent<NavMeshModifier>();
        fisicalMap[i, j].GetComponent<NavMeshModifier>().overrideArea = true;
        fisicalMap[i, j].GetComponent<NavMeshModifier>().area = 1;

    }

    public int isVerticalOrHorizontal(int i, int j) {//1 horizontal, 2 veertical, 0 quina
        int min = Mathf.Min(i, j);
        int max = Mathf.Max(i, j);

        if (dim - min < max)
        {
            if (max == i)
                return 2;
            return 1;
        } else if (dim - min > max) {
            if (min == i)
                return 2;
            return 1;
        }
        else {
            return 0;
        }
    }

    public int isEmptySpace(int i, int j) {
        if (map[i, j] == 0)
        {
            return 1;//Espaco Livre
        }
        return 0;//Tem algo nesse espaco
    }

    public Vector2 fromWorldToLab(Vector3 position) {

        int tam = tamLabirinto;
        float i = (position.z / tam) % 1;
        int iInt = (int)(position.z / tam);
        float j = (position.x / tam) % 1;
        int jInt = (int)(position.x / tam);
        int limites = ((dim - 1) / 2) * tam; //vai de -1 isso ate +1 isso
        //print((int)(position.z / tam));

        //cordenada i da matriz
        int corrPosiX = Mathf.Abs(i) <= 0.5f ? iInt : (iInt >= 0 ? iInt + 1 : iInt - 1); //recebe o o multiplo de 10 pra cima se for maior que 0,5 e pra baixo se menor que 0,5
        corrPosiX *= tam;
        int matPositionX = corrPosiX + limites;
        if (matPositionX >= 0)
        {
            i = matPositionX / tam;
        }

        //cordenada j da matriz
        int corrPosiY = Mathf.Abs(j) <= 0.5f ? jInt : (jInt >= 0 ? jInt + 1 : jInt - 1); //recebe o o multiplo de 10 pra cima se for maior que 0,5 e pra baixo se menor que 0,5
        corrPosiY *= tam;
        int matPositionY = corrPosiY + limites;
        if (matPositionY >= 0)
        {
            j = matPositionY / tam;
        }

        return new Vector2(i, j);
    }

    public Vector3 fromLabToWorld(Vector2 cord) {
        float i, j;
        float init = -1*(((dim - 1) * 10) / 2);//inicio do labirinto
        i = init + 10 * cord.x;
        j = init + 10 * cord.y;

        return new Vector3(j, 2, i);
    }
}
