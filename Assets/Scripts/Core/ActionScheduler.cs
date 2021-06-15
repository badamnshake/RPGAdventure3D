using UnityEngine;

// if there are 2 type of actions a player can do,
// movement and combat then this script 
// eliminates happening of both at the same time.
namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {

            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;

        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}