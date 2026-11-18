/** Mirrors ExerciseResponse on the backend. */
export interface Exercise {
  id: number;
  name: string;
  category: string | null;
  isCustom: boolean;
}
