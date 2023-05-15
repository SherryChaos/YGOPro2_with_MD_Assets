using UnityEngine;

public class toolShift : MonoBehaviour
{
    public GameObject ObjectMust;
    public GameObject ObjectOption;

    private void Update()
    {
        if (Program.InputEnterDown)
            if (ObjectMust.name == "input_")
            {
                var input = UIHelper.getByName<UIInput>(ObjectMust, "input_");
                if (input != null)
                {
                    if (ObjectMust.transform.localPosition.y < 0)
                    {
                        shift();
                        input.isSelected = true;
                    }
                    else
                    {
                        if (input.isSelected)
                        {
                            if (input.value == "")
                            {
                                shift();
                                input.isSelected = false;
                            }
                        }
                        else
                        {
                            input.isSelected = true;
                        }
                    }
                }
            }
    }

    public void shift()
    {
        var va = ObjectMust.transform.localPosition;
        if (ObjectOption == null)
        {
            if (va.y >= 0)
                iTween.MoveToLocal(ObjectMust, new Vector3(va.x, -70, va.z), 0.6f);
            else
                iTween.MoveToLocal(ObjectMust, new Vector3(va.x, 30, va.z), 0.6f);
        }
        else
        {
            var vb = ObjectOption.transform.localPosition;
            if (va.y > vb.y)
            {
                iTween.MoveToLocal(ObjectMust, new Vector3(va.x, -70, va.z), 0.6f);
                iTween.MoveToLocal(ObjectOption, new Vector3(vb.x, 30, vb.z), 0.6f);
            }
            else
            {
                iTween.MoveToLocal(ObjectMust, new Vector3(va.x, 30, va.z), 0.6f);
                iTween.MoveToLocal(ObjectOption, new Vector3(vb.x, -70, vb.z), 0.6f);
            }
        }
    }
}