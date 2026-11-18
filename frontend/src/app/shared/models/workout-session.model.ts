import { CreateSetEntryRequest, SetEntry } from './set-entry.model';

/** Mirrors WorkoutSessionSummaryResponse (list view, without sets). */
export interface WorkoutSessionSummary {
  id: number;
  /** ISO date (yyyy-MM-dd) */
  date: string;
  notes: string | null;
}

/** Mirrors WorkoutSessionResponse (detail view, with sets). */
export interface WorkoutSession extends WorkoutSessionSummary {
  sets: SetEntry[];
}

/** Mirrors CreateWorkoutSessionRequest. */
export interface CreateWorkoutSessionRequest {
  /** ISO date (yyyy-MM-dd) */
  date: string;
  notes?: string | null;
  /** At most 200 sets */
  sets?: CreateSetEntryRequest[] | null;
}
