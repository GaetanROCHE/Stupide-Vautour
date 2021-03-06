﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StupideVautour
{
    public partial class Table_jeu : Form
    {
        Mains Mains;
        int[] pioche_point;
        int Jeu_tour;
        int[] scores;
        int nbJoueurs;
        IA[] adversaires;
        int pointEnJeu;
        SoundPlayer snd = null;

        /*
         * Ce constructeur prend en paramètres :
         * le nombre de Joueurs entre 1 et 5
         * la difficulté des IA affrontés qui peut valoir :
         * 1 : Facile
         * 2 : Normal
         * 3 : Difficile
         * 42 : Impossible ^_-
         * 
         */
        public Table_jeu(int nbJoueurs, int difficulte)
        {
            InitializeComponent();
            this.nbJoueurs = nbJoueurs;
            Mains = new Mains(nbJoueurs);
            pioche_point = new int[15];
            scores = new int[nbJoueurs];

            initialiseAdversaires(nbJoueurs, difficulte);
            melangedeck(pioche_point, 15);
            Jeu_tour = 0;
            afficherMainJoueur();

            PictureBox cartePoint;
            cartePoint = (PictureBox)affiche_carte_point;
            if (pioche_point[Jeu_tour] < 0)
            {
                cartePoint.Image = Liste_cartes_points.Images[pioche_point[Jeu_tour]+5];
            }
            else
            {
                cartePoint.Image = Liste_cartes_points.Images[pioche_point[Jeu_tour]+4];
            }

            affichageScore();

            Stream str = Properties.Resources.InGameSound;
            snd = new SoundPlayer(str);
            snd.PlayLooping();
        }

        void initialiseAdversaires(int nb, int difficulte)
        {
            adversaires = new IA[5];
            int i;
            for (i = 1; i < nb; i++)
            {
                switch(difficulte)
                {
                    case 42:
                        adversaires[i] = new IAJohn(i, nbJoueurs);
                        break;
                    case 2:
                        adversaires[i] = new IAnormal(i, nbJoueurs);
                        break;
                    case 3:
                        adversaires[i] = new IAdifficile(i, nbJoueurs);
                        break;
                    default:
                        adversaires[i] = new IAnovice(i, nbJoueurs);
                        break;
                }
                
            }
        }
        public void affichageScore()
        {
            label0.Text = "Score : " + scores[0];
            label1.Text = "Score : " + scores[1];
            if (nbJoueurs > 2)
            {
                label2.Text = "Score : " + scores[2];
                if (nbJoueurs > 3)
                {
                    label3.Text = "Score : " + scores[3];
                    if(nbJoueurs > 4)
                        label4.Text = "Score : " + scores[4];
                }
            }
        }

        void afficherMainJoueur()
        {
            PictureBox carte;
            int k;
            for (k = 0; k < Affiche_Main_joueur.ColumnCount; k++)
            {
                carte = (PictureBox)Affiche_Main_joueur.GetControlFromPosition(k, 0);
                if (Mains.existe(0,k+1))
                {
                    carte.Image = Liste_cartes_jouer.Images[k];
                }
                else
                {
                    carte.Image = null;
                }
            }
        }

        void melangedeck(int[] tabl, int taille)
        {
            int i;
            int j;
            Random alea;
            int[] trie = { -5, -4, -3, -2, -1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            for (i = 0; i < 15; i++)
            {
                do
                {
                    alea = new Random();
                    j = alea.Next(15);
                } while (trie[j] == 0);
                tabl[i] = trie[j];
                trie[j] = 0;
            }
        }

        private void Affiche_Main_joueur_Click(object sender, EventArgs e)
        {
        }

        private bool testFin()
        {
            int i;
            for(i=1;i<=15;i++)
            {
                if(Mains.existe(0,i))
                {
                    return false;
                }
            }
            return true;
        }

        /**
         * @c'est ici qu'on compte les points
         */
        private void compte(int carteJoueur, int[] carteIA, int point)
        {
            int i;
            int[] cartesJouees = new int[15]; //0 : personnes ne l'a jouée // 1 à 5 : 1joueurs l'a jouée // 6 : plusieurs personnes l'on jouée
            for (i = 0; i < 15; i++)
            {
                cartesJouees[i] = 0;
            }
            cartesJouees[carteJoueur-1] = 1;
            for (i = 0; i < nbJoueurs-1; i++)
            {
                if (cartesJouees[carteIA[i]] != 0)
                {
                    cartesJouees[carteIA[i]-1] = 6;
                }
                else
                {
                    cartesJouees[carteIA[i]-1] = i + 1;
                }
            }
            if (point > 0)
            {
                int j = 14;
                while (j != 0 && (cartesJouees[j] == 0 || cartesJouees[j] == 6))
                {
                    j--;
                }
                if (j != 0)
                {
                    scores[cartesJouees[j] - 1] = +point;
                }
            }
            else
            {
                int j = 0;
                while (j != 15 && (cartesJouees[j] == 0 || cartesJouees[j] == 6))
                {
                    j++;
                }
                if (j != 15)
                {
                    scores[cartesJouees[j] - 1] = +point;
                }
            }
        }

        private void ramasse()
        {
            PictureBox carteJouee0;
            carteJouee0 = (PictureBox)carte_jouer;
            carteJouee0.Image = null;

            PictureBox carteJouee1;
            carteJouee1 = (PictureBox)carte_jouer_IA1;
            carteJouee1.Image = null;

            PictureBox carteJouee2;
            carteJouee2 = (PictureBox)carte_jouer_IA2;
            carteJouee2.Image = null;

            PictureBox carteJouee3;
            carteJouee3 = (PictureBox)carte_jouer_IA3;
            carteJouee3.Image = null;

            PictureBox carteJouee4;
            carteJouee4 = (PictureBox)carte_jouer_IA4;
            carteJouee4.Image = null;

            Console.WriteLine("score = {0}", scores[0]);
            affichageScore();
        }


        private void joueurJoue(int carte)
        {
            int i;
            int[] IAjoue = new int[nbJoueurs-1];
            if(Mains.existe(0,carte))
            {
                // Tout le monde joue :
                Mains.joue(0, carte);
                for (i = 1; i < nbJoueurs; i++)
                {
                    Mains.joue(i, IAjoue[i - 1] = adversaires[i].Joue(Mains, pioche_point[Jeu_tour], scores));
                }

                // On affiche Le tout un petit moment :
                afficherMainJoueur();

                PictureBox carteJouee0;
                carteJouee0 = (PictureBox)carte_jouer;
                carteJouee0.Image = Liste_cartes_jouer.Images[carte - 1];

                PictureBox carteJouee1;
                carteJouee1 = (PictureBox)carte_jouer_IA1;

                carteJouee1.Image = Liste_cartes_jouer.Images[IAjoue[0] - 1];

                if (nbJoueurs > 2)
                {
                    PictureBox carteJouee2;
                    carteJouee2 = (PictureBox)carte_jouer_IA2;

                    carteJouee2.Image = Liste_cartes_jouer.Images[IAjoue[1] - 1];

                    if (nbJoueurs > 3)
                    {
                        PictureBox carteJouee3;
                        carteJouee3 = (PictureBox)carte_jouer_IA3;

                        carteJouee3.Image = Liste_cartes_jouer.Images[IAjoue[2] - 1];

                        if (nbJoueurs > 4)
                        {
                            PictureBox carteJouee4;
                            carteJouee4 = (PictureBox)carte_jouer_IA4;

                            carteJouee4.Image = Liste_cartes_jouer.Images[IAjoue[3] - 1];

                        }
                    }
                }

                if (Jeu_tour == 14)
                {
                    // On regarde le gagnant !
                    int max = scores[0];
                    for(i = 1; i < nbJoueurs; ++i)
                    {
                        if (scores[i] > max)
                            max = scores[i];
                    }

                    string caption = (scores[0] >= max) ? "Vous avez gagner ! :D" : "Vous avez perdu :(";
                    string message = "Votre partie est finie, voulez-vous recommencer?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);

                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        // Closes the parent form.
                        this.Close();
                    }
                }

                this.Refresh();
                System.Threading.Thread.Sleep(3000);
                compte(carte, IAjoue, pioche_point[Jeu_tour]);
                ramasse();
                Jeu_tour++;
                if (Jeu_tour < 14)
                {
                    PictureBox cartePoint;
                    cartePoint = (PictureBox)affiche_carte_point;
                    if (pioche_point[Jeu_tour] < 0)
                    {
                        cartePoint.Image = Liste_cartes_points.Images[pioche_point[Jeu_tour] + 5];
                    }
                    else
                    {
                        cartePoint.Image = Liste_cartes_points.Images[pioche_point[Jeu_tour] + 4];
                    }
                }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            joueurJoue(1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            joueurJoue(2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            joueurJoue(3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            joueurJoue(4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            joueurJoue(5);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            joueurJoue(6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            joueurJoue(7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            joueurJoue(8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            joueurJoue(9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            joueurJoue(10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            joueurJoue(11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            joueurJoue(12);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            joueurJoue(13);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            joueurJoue(14);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            joueurJoue(15);
        }


    }
}
