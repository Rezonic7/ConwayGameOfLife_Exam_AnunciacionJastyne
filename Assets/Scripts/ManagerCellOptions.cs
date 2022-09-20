using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerCellOptions : MonoBehaviour
{
    private Color aliveColor = Color.white;
    private Color deadColor = Color.black;

    private int columnSize = 60;
    private int rowSize = 33;

    private float generationSpeed = 0.1f;

    [SerializeField] private Dropdown dropDownAliveColors;
    [SerializeField] private Dropdown dropDowndeadColors;

    [SerializeField] private InputField inputFieldColumnSize;
    [SerializeField] private InputField inputFieldRowSize;

    [SerializeField] private Slider sliderGenerationSpeed;

    private IChangeableSettingsCellMatrix iChangeableSettingsCellMatrix;
    private void Awake()
    {
        foreach (GameObject gameObjects in FindObjectsOfType<GameObject>())
        {
            if (gameObjects.GetComponent<IChangeableSettingsCellMatrix>() != null)
                iChangeableSettingsCellMatrix = gameObjects.GetComponent<IChangeableSettingsCellMatrix>(); 
        }
    }
    private void Start()
    {
        DefaultOptions();

        if(PlayerPrefs.HasKey("AliveCellColor") && PlayerPrefs.HasKey("DeadCellColor") && PlayerPrefs.HasKey("ColumnSize")
            && PlayerPrefs.HasKey("RowSize") && PlayerPrefs.HasKey("GenerationSpeed"))
        {
            SetPrefsValue();
        }

        ApplyChanges();
    }
    public void DefaultOptions()
    {
        dropDownAliveColors.value = 0;
        dropDowndeadColors.value = 0;

        aliveColor = ChangeAliveColors(dropDownAliveColors.value);
        deadColor = ChangeAliveColors(dropDowndeadColors.value);

        columnSize = 60;
        inputFieldColumnSize.text = columnSize.ToString();
        rowSize = 33;
        inputFieldRowSize.text = rowSize.ToString();

        generationSpeed = 0.1f;
    }

    private void SetPrefsValue()
    {
        dropDownAliveColors.value = PlayerPrefs.GetInt("AliveCellColor");
        dropDowndeadColors.value = PlayerPrefs.GetInt("DeadCellColor");

        aliveColor = ChangeAliveColors(dropDownAliveColors.value);
        deadColor = ChangeDeadColors(dropDowndeadColors.value);

        columnSize = PlayerPrefs.GetInt("ColumnSize");
        inputFieldColumnSize.text = columnSize.ToString();
        rowSize = PlayerPrefs.GetInt("RowSize");
        inputFieldRowSize.text = rowSize.ToString();

        generationSpeed = PlayerPrefs.GetFloat("GenerationSpeed");
        sliderGenerationSpeed.value = generationSpeed;

    }

    public void Restart()
    {
        PlayerPrefs.SetInt("AliveCellColor", dropDownAliveColors.value);
        PlayerPrefs.SetInt("DeadCellColor", dropDowndeadColors.value);

        PlayerPrefs.SetInt("ColumnSize", int.Parse(inputFieldColumnSize.text));
        PlayerPrefs.SetInt("RowSize", int.Parse(inputFieldRowSize.text));

        PlayerPrefs.SetFloat("GenerationSpeed", sliderGenerationSpeed.value);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ApplyChanges()
    {
        aliveColor = ChangeAliveColors(dropDownAliveColors.value);
        deadColor = ChangeDeadColors(dropDowndeadColors.value);

        columnSize = int.Parse(inputFieldColumnSize.text);
        rowSize = int.Parse(inputFieldRowSize.text);

        generationSpeed = sliderGenerationSpeed.value;

        iChangeableSettingsCellMatrix.SetVariables(aliveColor, deadColor, columnSize, rowSize, generationSpeed);
    }

    public Color ChangeAliveColors(int dropdownOptions)
    {
        Color colorToSet = Color.white;

        switch (dropdownOptions)
        {
            case 0:
                colorToSet = Color.white;
                break;
            case 1:
                colorToSet = Color.black;
                break;
            case 2:
                colorToSet = Color.blue;
                break;
            case 3:
                colorToSet = Color.yellow;
                break;
            case 4:
                colorToSet = Color.red;
                break;
            case 5:
                colorToSet = Color.green;
                break;
        }
        return colorToSet;
    }

    public Color ChangeDeadColors(int dropdownOptions)
    {
        Color colorToSet = Color.black;
        switch (dropdownOptions)
        {
            case 0:
                colorToSet = Color.black;
                break;
            case 1:
                colorToSet = Color.white;
                break;
            case 2:
                colorToSet = Color.blue;
                break;
            case 3:
                colorToSet = Color.yellow;
                break;
            case 4:
                colorToSet = Color.red;
                break;
            case 5:
                colorToSet = Color.green;
                break;
        }


        return colorToSet;
    }
}
