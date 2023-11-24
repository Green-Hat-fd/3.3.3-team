using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManager : MonoBehaviour
{
    TextAsset saveTxt;

    [Header("—— Variabili delle informazioni ——")]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] OptionsSO_Script opt_SO;

    [Space(20)]
    [SerializeField] string fileName = "utility";
    string file_path;

    private readonly string encryptionKey = "bLVHaYVi8y";

    const string STATS_TITLE = "# STATS #",
                 SCORE_TITLE = "# SCORE #",
                 OTHERS_TITLE = "# OTHERS #",
                 OPTIONS_TITLE = "# OPTIONS #";
    
    
    
    private void Awake()
    {
        //Prende il percorso dell'applicazione
        file_path = Application.dataPath + "/" + fileName + ".txt";
    }



    public void SaveGame()
    {
        string saveString = "";


        #region -- Stats Giocatore --

        saveString += STATS_TITLE + "\n";

        //Aggiunge "checkpoint" (livello e posizione)
        saveString += SceneManager.GetActiveScene().buildIndex + "\n";
        saveString += stats_SO.GetCheckpointPos().x + "\n";
        saveString += stats_SO.GetCheckpointPos().y + "\n";
        saveString += stats_SO.GetCheckpointPos().z + "\n";

        #endregion


        #region -- Punteggio --

        saveString += "\n" + SCORE_TITLE + "\n";

        saveString += stats_SO.GetScore() + "\n";   //Aggiunge il punteggio

        #endregion


        #region -- Opzioni --

        saveString += "\n" + OPTIONS_TITLE + "\n";

        //Aggiunge tutte le opzioni scelte
        saveString += opt_SO.GetMusicVolume_Percent() + "\n";
        saveString += opt_SO.GetSoundVolume_Percent() + "\n";
        saveString += opt_SO.GetIsFullscreen() + "\n";

        #endregion


        #region -- Altro --

        saveString += "\n" + OTHERS_TITLE + "\n";

        foreach (bool isButterflyTaken in stats_SO.GetButterflyCollected())
        {
            saveString += isButterflyTaken + "\n";   //Aggiunge ogni farfalla
        }

        #endregion


        //Codifica il file
        //(Encryption)
        saveString = EncryptDecrypt(saveString);


        //Sovrascrive il file
        //(se non esiste, ne crea uno nuovo e ci scrive)
        File.WriteAllText(file_path, saveString);



        #region Prodotto finale
        /*  0:  ### STATS ###
         *  1:  Livello
         *  2:  Checkpoint - Posizione X
         *  3:  Checkpoint - Posizione Y
         *  4:  Checkpoint - Posizione Z
         *  5:  
         *  6:  ### SCORE ###
         *  7:  Punteggio (tot)
         *  8:  
         *  9:  ### OPTIONS ###
         * 10:  Volume musica
         * 11:  Volume effetti sonori
         * 12:  Schermo intero
         * 13:  
         * 14:  ### OTHERS ###
         * 15:  Collez. 1 raccolto
         * 16:  Collez. 2 raccolto
         * 17:  Collez. 3 raccolto
         * 18:  Collez. 4 raccolto
         * 19:  
         * 20:  
         * 21:  
         * 22:  
         * 23:  
         * 24:  
         * 25:  
         * 26:  
         * 27:  
         * 28:  
         * 29:  
         * 30:  
         * 31:  
         * 32:  
         * 33:  
         */  
        #endregion
    }


    public void LoadGame()
    {
        string[] fileReading;

        int i_stats = 0,
            i_score = 0,
            i_options = 0,
            i_others = 0;


        //Se il file esiste...
        if (File.Exists(file_path))
        {
            //Legge il file di salvataggio
            string encriptedText = File.ReadAllText(file_path);


            //Decodifica ogni stringa dell'array
            //(Decryption)
            encriptedText = EncryptDecrypt(encriptedText);


            //Lo divide ogni "a capo"
            //e lo sposta nell'array da leggere
            fileReading = encriptedText.Split('\n');
        }
        else
        {
            print("[!] Messaggio di errore");
            return;
        }


        #region Trovare i punti di inizio

        //Cerca nell'array i punti di inizio delle varie "regioni"
        for (int i = 0; i < fileReading.Length; i++)
        {
            switch (fileReading[i])
            {
                case STATS_TITLE:
                    i_stats = i;
                    break;

                case SCORE_TITLE:
                    i_score = i;
                    break;

                case OPTIONS_TITLE:
                    i_options = i;
                    break;

                case OTHERS_TITLE:
                    i_others = i;
                    break;
            }
        }

        #endregion



        #region -- Stats Giocatore --

        //Trasforma da string a int
        int level_load = int.Parse(fileReading[i_stats + 1]);
        float posX_load = float.Parse(fileReading[i_stats + 2]),
              posY_load = float.Parse(fileReading[i_stats + 3]),
              posZ_load = float.Parse(fileReading[i_stats + 4]);

        //Carica il livello e l'ultima posizione del giocatore
        //(e avvisa che si sta caricando dal livello)
        opt_SO.OpenChosenScene(level_load);
        stats_SO.LoadCheckpointPos(new Vector3(posX_load, posY_load, posZ_load));

        #endregion


        #region -- Punteggio --

        //Trasforma da string a int
        int score_load = int.Parse(fileReading[i_score + 1]);

        //Load del punteggio (tot)
        stats_SO.LoadScore(score_load);

        #endregion


        #region -- Opzioni --

        //Trasforma da string a int
        float musicVol_load = float.Parse(fileReading[i_options + 1]),
              soundVol_load = float.Parse(fileReading[i_options + 2]);
        bool fullscreen_load = bool.Parse(fileReading[i_options + 3]);

        //Load di tutte le opzioni
        opt_SO.ChangeMusicVolume(musicVol_load);
        opt_SO.ChangeSoundVolume(soundVol_load);
        opt_SO.ToggleFullscreen(fullscreen_load);

        #endregion


        #region -- Altro --

        for (int b = 0; b < stats_SO.GetButterflyCollected().Count; b++)
        {
            //Trasforma da bool a int per ogni farfalla
            bool butterfly_load = bool.Parse(fileReading[i_others + b + 1]);

            //Load di ogni farfalla
            stats_SO.LoadButterflyCollected(b, butterfly_load);
        }

        #endregion
    }


    public void GenerateNewGame()
    {
        //Cancella il file precedente
        DeleteSaveFile();

        //Salva in un nuovo file
        SaveGame();
    }

    public void DeleteSaveFile()
    {
        //Se già esiste, lo elimina
        if (File.Exists(file_path))
            File.Delete(file_path);
    }


    string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        int keyLenght = encryptionKey.Length;

        //Per ogni carattere della stringa...
        for (int i = 0; i < data.Length; i++)
        {
            //Cambia con un'operazione XOR, scambiando ogni carattere con un'altro
            //(operazione reversibile se si usa la stessa chiave/key)
            modifiedData += (char)(data[i] ^ encryptionKey[i % keyLenght]);
        }

        return modifiedData;
    }
}