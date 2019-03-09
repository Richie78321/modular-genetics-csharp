using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularGenetics
{
    public abstract class FitnessEvolver<D>
    {
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

        public FitnessEvolver(ModularMember modularMember)
        {
            SetModularMember(modularMember);
        }

        public abstract double DetermineFitness();

        private ModularMember modularMember;
        public ModularMember ModularMember => modularMember;
        private void SetModularMember(ModularMember modularMember)
        {
            if (!modularMember.GenomeAssigned) throw new Exception("Genome has not been assigned to modular member!");
            this.modularMember = modularMember;
        }

        protected abstract D GetData();

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
