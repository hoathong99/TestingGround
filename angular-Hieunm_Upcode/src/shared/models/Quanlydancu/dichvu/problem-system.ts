export interface ProblemSystemDto {
    id?: number;
    name?: string;
    typeProblem?: number;
    state?: number;
    giverId?: number;
    performerId?: number;
    description?: string;
    timeStart?: Date;
    timeFinish?: Date;
}