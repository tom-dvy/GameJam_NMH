using UnityEngine;

public class MenueManager : MonoBehaviour
{
    // On déclare les deux objets (les "Empty" qui servent de conteneurs)
    public GameObject menuPrincipal;
    public GameObject interfaceJeu;

    // Cette fonction sera appelée par votre bouton
    public void LancerJeu()
    {
        // On désactive le menu
        menuPrincipal.SetActive(false);

        // On active le jeu
        interfaceJeu.SetActive(true);
    }
}