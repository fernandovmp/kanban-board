import { SummarizedTask } from '.';

export interface TaskList {
    id: number;
    title: string;
    tasks: SummarizedTask[];
}
