﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StupideVautour
{
    public partial class Table_jeu : Form
    {
        int[,] mains_joueurs;
        int[] pioche_point;
        int[] cartes_tombee;
        int Jeu_tour;
        int Score;
        int nbJoueurs;
        IA[] adversaires;


        public Table_jeu(int nbJoueurs)
        {
            InitializeComponent();
            this.nbJoueurs = nbJoueurs;
            mains_joueurs = new int[nbJoueurs,15];
            pioche_point = new int[15];

            initialiseAdversaires(nbJoueurs);
            initialiseMainsJoueurs(nbJoueurs);
            melangedeck(pioche_point, 15);
            Jeu_tour = 0;
            afficherMainJoueur();

            PictureBox cartePoint;
            cartePoint = (PictureBox)affiche_carte_point;
            cartePoint.Image = Liste_cartes_points.Images[Jeu_tour];

        }

        void initialiseAdversaires(int nb)
        {
            adversaires = new IA[5];
            int i;
            for (i = 1; i < nb; i++)
            {
                adversaires[i] = new IA(i);
            }
        }

        void initialiseMainsJoueurs(int nbJoueurs)
        {
            int i;
            int j;
            for (i = 0; i < nbJoueurs; i++)
            {
                for (j = 0; j < 15; j++)
                {
                    mains_joueurs[i, j] = 1;
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
                if (mains_joueurs[0, k] == 1)
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
            for(i=0;i<15;i++)
            {
                if (mains_joueurs[0, i] == 1)
                {
                    return false;
                }
            }
            return true;
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
        }


        private void joueurJoue(int carte)
        {
            int i;
            int[] IAjoue = new int[nbJoueurs];
            if (mains_joueurs[0, carte - 1] == 1)
            {
                // Tout le monde joue :
                mains_joueurs[0, carte - 1] = 0;

                for (i = 1; i < nbJoueurs; i++)
                {
                    IAjoue[i] = adversaires[i].Joue(mains_joueurs, cartes_tombee);
                }

                // On affiche Le tout un petit moment :
                afficherMainJoueur();

                PictureBox carteJouee0;
                carteJouee0 = (PictureBox)carte_jouer;
                carteJouee0.Image = Liste_cartes_jouer.Images[carte - 1];

                PictureBox carteJouee1;
                carteJouee1 = (PictureBox)carte_jouer_IA1;
                carteJouee1.Image = Liste_cartes_jouer.Images[IAjoue[1]];

                if (nbJoueurs > 2)
                {
                    PictureBox carteJouee2;
                    carteJouee2 = (PictureBox)carte_jouer_IA2;
                    carteJouee2.Image = Liste_cartes_jouer.Images[IAjoue[2]];
                    if (nbJoueurs > 3)
                    {
                        PictureBox carteJouee3;
                        carteJouee3 = (PictureBox)carte_jouer_IA3;
                        carteJouee3.Image = Liste_cartes_jouer.Images[IAjoue[3]];
                        if (nbJoueurs > 4)
                        {
                            PictureBox carteJouee4;
                            carteJouee4 = (PictureBox)carte_jouer_IA4;
                            carteJouee4.Image = Liste_cartes_jouer.Images[IAjoue[4]];
                        }
                    }
                }

                

                if (Jeu_tour == 14)
                {
                    string caption = "Partie finie";
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
                ramasse();
                Jeu_tour++;
                if (Jeu_tour > 14)
                {
                    PictureBox cartePoint;
                    cartePoint = (PictureBox)affiche_carte_point;
                    cartePoint.Image = Liste_cartes_points.Images[Jeu_tour];
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