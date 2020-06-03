using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageFolderManager : MonoBehaviour
{
    public string inPath;
    public string outPath;

    private DirectoryInfo dirIN;
    private DirectoryInfo dirOUT;

    FileInfo[] fileInfosIN;
    FileInfo[] fileInfosOUT;

    int filesLoaded;
    int filesNotLoaded;

    bool IsAvailable;

    void Start()
    {
        dirIN = new DirectoryInfo(inPath);
        dirOUT = new DirectoryInfo(outPath);
    }

    void Update()
    {
        fileInfosIN = dirIN.GetFiles("*.png");
        fileInfosOUT = dirOUT.GetFiles("*.png");

        filesNotLoaded = fileInfosOUT.Length;

        if(filesNotLoaded > 0)
        {
            IsAvailable = true;

            if(filesNotLoaded != filesLoaded)
            {
                if(filesNotLoaded > filesLoaded)
                {
                    LoadAllImage();
                }
                else if(filesNotLoaded < filesLoaded)
                {
                    if(RemovedFile() == null)
                    {
                        Debug.Log("Working...");
                    }
                    else
                    {
                        string temp_thatFile = RemovedFile().Name;

                        File.Delete(Path.Combine(inPath,(temp_thatFile + ".meta")));
                        File.Delete(Path.Combine(inPath,temp_thatFile));
                    }
                }

                UpdateFilesLoaded();
            }
        }
        else
        {
            IsAvailable = false;

            if(filesLoaded > 0)
            {
                UnloadAllImage();

                UpdateFilesLoaded();
            }
        }
    }

    private void UpdateFilesLoaded(){filesLoaded = filesNotLoaded;}

    FileInfo RemovedFile()
    {
        foreach(FileInfo fileIN in fileInfosIN)
        {
            bool temp_match = false;

            foreach(FileInfo fileOUT in fileInfosOUT)
            {
                if(fileIN.Name == fileOUT.Name)
                {
                    temp_match = true;
                    break;
                }
            }

            if(!temp_match)
            {
                return fileIN;
            }
        }

        return null;
    }

    public void LoadAllImage()
    {
        int temp_loadAmount = 0;

        foreach(FileInfo file in fileInfosOUT)
        {
            Debug.Log("Loading " + ((temp_loadAmount/(filesNotLoaded - filesLoaded))*100).ToString() + "%");

            if(!File.Exists(Path.Combine(inPath,file.Name)))
            {
                File.Copy(Path.Combine(outPath,file.Name),Path.Combine(inPath,file.Name));
            }

            temp_loadAmount++;
        }

        Debug.Log("Loaded!");
    }

    public void UnloadAllImage()
    {
        int temp_delAmount = 0;

        foreach(FileInfo file in fileInfosIN)
        {
            Debug.Log("Deleting " + ((temp_delAmount/(filesLoaded - filesNotLoaded))*100).ToString() + "%");

            File.Delete(Path.Combine(inPath,(file.Name + ".meta")));
            File.Delete(Path.Combine(inPath,file.Name));

            temp_delAmount++;
        }

        Debug.Log("Deleted!");
    }
}