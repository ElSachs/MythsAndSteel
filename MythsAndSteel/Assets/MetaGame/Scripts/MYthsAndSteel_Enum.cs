﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYthsAndSteel_Enum : MonoBehaviour
{
    /// <summary>
    /// Enum pour le type des unités
    /// </summary>
    public enum TypeUnite{ 
        Infanterie, Vehicule, Artillerie, Mythe, Leader, Mecha
    }

    /// <summary>
    /// Enum pour els phases du jeu
    /// </summary>
    public enum PhaseDeJeu{
        Debut, Activation, OrgoneJ1, ActionJ1, OrgoneJ2, ActionJ2, Strategie
    }

    /// <summary>
    /// Enum pour les cartes events
    /// </summary>
    public enum EventCard{
        Pointeurs_laser_optimisés, Fil_barbelé, Déploiement_accéléré, Activation_de_nodus, Réapprovisionnement, Illusion_stratégique, Détonation_d_orgone, Bombardement_aérien, 
        Sérum_expérimental, Transfusion_d_orgone, Optimisation_de_l_orgone, Pillage_orgone, Manoeuvre_stratégique, Reprogrammation, Paralysie, Sabotage,
        Vol_de_ravitaillement, Arme_épidémiologique, Cessez_le_feu, Armes_perforantes, Entraînement_rigoureux,
        J1Faction1, J1Faction2, J1Faction3, J2Faction1, J2Faction2, J2Faction3
    }

    /// <summary>
    /// Tous les types de terrain
    /// </summary>
    public enum TerrainType{
        Sol, Forêt, Bosquet, Plage, Colline, Haute_colline, Mont, Ravin, Eau, Boue, Rivière_Nord, Rivière_Sud, Rivière_Est, Rivière_Ouest, Fleuve, Ruisseau, Pont_Nord, Pont_Sud, Pont_Est, Pont_Ouest, Route, Rails, Maison, Immeuble, Ruines, Usine, Point_de_ressource, Gare, Bunker, Cabane_Isatabu, Point_Objectif, OrgoneBlue, OrgoneRed,
    }

    /// <summary>
    /// Enum pour savoir qui possède un objectif
    /// </summary>
    public enum Owner{
        neutral, blue, red
    }

    /// <summary>
    /// Status obtenable par une unité
    /// </summary>
    public enum Statut{
        ArmeEpidemiologique, Paralysie, Immobilisation, Réanimé, Possédé, Invincible, PeutPasCombattre, PeutPasPrendreDesObjectifs 
    }
}
