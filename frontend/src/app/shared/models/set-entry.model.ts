/** Mirrors SetEntryResponse on the backend. */
export interface SetEntry {
  id: number;
  exerciseId: number;
  setNumber: number;
  weight: number;
  reps: number;
  rpe: number | null;
  isWarmup: boolean;
}

/** Mirrors SetHistoryResponse: one set of an exercise, with the session it belongs to. */
export interface SetHistory {
  id: number;
  sessionId: number;
  /** ISO date (yyyy-MM-dd) */
  date: string;
  setNumber: number;
  weight: number;
  reps: number;
  rpe: number | null;
  isWarmup: boolean;
}

/** Mirrors CreateSetEntryRequest. */
export interface CreateSetEntryRequest {
  exerciseId: number;
  /** 1-100 */
  setNumber: number;
  /** 0-1000 */
  weight: number;
  /** 0-1000 */
  reps: number;
  /** 0-10 */
  rpe?: number | null;
  isWarmup: boolean;
}

/** Mirrors UpdateSetEntryRequest. */
export interface UpdateSetEntryRequest {
  setNumber: number;
  weight: number;
  reps: number;
  rpe?: number | null;
  isWarmup: boolean;
}
