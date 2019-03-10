using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public abstract class FitnessEvolver<D>
    {
        /// <summary>
        /// Evolves a generation of FitnessEvolvers.
        /// </summary>
        /// <typeparam name="T">The type of FitnessEvolvers to be evolved.</typeparam>
        /// <param name="generation">The generation of FitnessEvolvers.</param>
        /// <param name="selectionPercentage">The percentage of the fittest within the generation that will be selected for breeding in the next generation.</param>
        /// <param name="mutationRate">The mutation rate to be used while breeding the next generation.</param>
        /// <param name="fittestMember">The fittest member of the evaluated generation.</param>
        /// <param name="random">The random to be used for breeding and selection.</param>
        /// <param name="multithreaded">Whether the evaluation processes should happen on multiple threads.</param>
        /// <returns>Returns the evolved generation.</returns>
        public static T[] EvolveGeneration<T>(T[] generation, double selectionPercentage, double mutationRate, out MemberEvaluation<T, D> fittestMember, Random random, bool multithreaded = false) where T : FitnessEvolver<D>, new()
        {
            //Evaluate members
            MemberEvaluation<T, D>[] memberEvaluations = new MemberEvaluation<T, D>[generation.Length];
            if (multithreaded) Parallel.For(0, generation.Length, (i) => { memberEvaluations[i] = new MemberEvaluation<T, D>(generation[i], generation[i].DetermineFitness()); });
            else for (int i = 0; i < generation.Length; i++) memberEvaluations[i] = new MemberEvaluation<T, D>(generation[i], generation[i].DetermineFitness());

            //TEST AVERAGE
            double average = 0;
            for (int i = 0; i < memberEvaluations.Length; i++) average += memberEvaluations[i].Fitness;
            average /= memberEvaluations.Length;
            Console.WriteLine(average);

            //Sort members
            Array.Sort(memberEvaluations, delegate (MemberEvaluation<T, D> x, MemberEvaluation<T, D> y) { return y.Fitness.CompareTo(x.Fitness); });

            //Select top
            int numToSelect = (int)Math.Ceiling(selectionPercentage * generation.Length);
            MemberEvaluation<T, D>[] selectedMembers = new MemberEvaluation<T, D>[numToSelect];
            for (int i = 0; i < numToSelect; i++) selectedMembers[i] = memberEvaluations[i];
            fittestMember = memberEvaluations[0];

            //Breed remaining
            List<T> newGeneration = new List<T>(generation.Length);
            for (int i = 0; i < selectedMembers.Length; i++) newGeneration.Add(selectedMembers[i].FitnessMember);
            while (newGeneration.Count < generation.Length)
            {
                T parent1 = WeightedSelect(selectedMembers, random);
                T parent2 = WeightedSelect(selectedMembers, random);
                D parent1Data = parent1.GetData();
                D parent2Data = parent2.GetData();

                ModularMember[] childGenomes = ModularMember.BreedMembers(parent1.ModularMember, parent2.ModularMember, mutationRate, random);
                for (int i = 0; i < childGenomes.Length && newGeneration.Count < generation.Length; i++)
                {
                    T child = new T();
                    child.SetModularMember(childGenomes[i]);
                    child.SetData(parent1Data, parent2Data);
                    newGeneration.Add(child);
                }
            }

            return newGeneration.ToArray();
        }

        /// <summary>
        /// Performs a weighted selection of a set of evaluated members.
        /// </summary>
        /// <typeparam name="T">The type of FitnessEvolver.</typeparam>
        /// <param name="selectedMembers">The set of evaluated members.</param>
        /// <param name="random">The random to be used for selection.</param>
        /// <returns>Returns the selected FitnessEvolver</returns>
        private static T WeightedSelect<T>(MemberEvaluation<T, D>[] selectedMembers, Random random) where T : FitnessEvolver<D>
        {
            double totalFitness = 0;
            for (int i = 0; i < selectedMembers.Length; i++) totalFitness += selectedMembers[i].Fitness;

            double selectionValue = random.NextDouble();
            double totalSelectingFitness = 0;
            for (int i = 0; i < selectedMembers.Length; i++)
            {
                totalSelectingFitness += selectedMembers[i].Fitness;
                if (totalSelectingFitness / totalFitness >= selectionValue) return selectedMembers[i].FitnessMember;
            }

            throw new Exception("ERROR IN SELECTION!");
        }

        //Object
        protected FitnessEvolver() { }

        /// <summary>
        /// Creates a fitness evolver with a specified ModularMember.
        /// </summary>
        /// <param name="modularMember">The ModularMember to be used.</param>
        public FitnessEvolver(ModularMember modularMember)
        {
            SetModularMember(modularMember);
        }

        /// <summary>
        /// The task used to determine the fitness of the ModularMember.
        /// </summary>
        /// <returns>Returns the fitness of the ModularMember (higher is better).</returns>
        public abstract double DetermineFitness();

        private ModularMember modularMember;
        public ModularMember ModularMember => modularMember;
        /// <summary>
        /// Sets the ModularMember (used for breeding).
        /// </summary>
        /// <param name="modularMember">The ModularMember to be set.</param>
        private void SetModularMember(ModularMember modularMember)
        {
            if (!modularMember.GenomeAssigned) throw new Exception("Genome has not been assigned to modular member!");
            this.modularMember = modularMember;
        }

        /// <summary>
        /// Gets the data that should be passed from parent to child.
        /// </summary>
        /// <returns>Returns the data.</returns>
        protected abstract D GetData();

        /// <summary>
        /// Sets the data that should be given from parents.
        /// </summary>
        /// <param name="parent1Data">The first parent's data.</param>
        /// <param name="parent2Data">The second parent's data.</param>
        protected abstract void SetData(D parent1Data, D parent2Data);
    }

    public struct MemberEvaluation<T, D> where T : FitnessEvolver<D>
    {
        public readonly T FitnessMember;
        public readonly double Fitness;

        public MemberEvaluation(T fitnessMember, double fitness)
        {
            FitnessMember = fitnessMember;
            Fitness = fitness;
        }
    }
}
