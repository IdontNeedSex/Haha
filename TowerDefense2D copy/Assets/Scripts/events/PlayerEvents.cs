using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    //TODO_done: You can look at the other Event classes located at "Assets/Scripts/events"
    
    //TODO_done: Define a delegate with the following signature "void OnPlayerMoneyChanged(int curMoney)"

    //TODO_done: Define a corresponding event named "OnPlayerMoneyChangedEvent"

    //TODO_done: Define a delegate with the following signature "void OnPlayerHealthChanged(int curHealth)"

    //TODO_done: Define a corresponding event named "OnPlayerHealthChangedEvent"
    
    public delegate void OnPlayerMoneyChanged(int curMoney);
    
    public event OnPlayerMoneyChanged OnPlayerMoneyChangedEvent;

    public delegate void OnPlayerHealthChanged(int curhealth);

    public event OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    
    

    public void TriggerOnPlayerMoneyChangedEvent(int curMoney)
    {
        Debug.Log("EVENT: Player Money changed, new amount=" + curMoney);
        OnPlayerMoneyChangedEvent?.Invoke(curMoney);
        //TODO_DONE: Invoke the "OnPlayerMoneyChangedEvent"
    }

    public void TriggerOnPlayerHealthChanged(int curHealth)
    {
        Debug.Log("EVENT: Player Health changed, new amount=" + curHealth);
        //TODO_done: Invoke the "OnPlayerHealthChangedEvent"
        OnPlayerHealthChangedEvent?.Invoke(curHealth);
    }
}