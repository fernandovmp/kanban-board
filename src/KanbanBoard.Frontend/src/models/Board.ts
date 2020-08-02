import { TaskList } from './TaskList';

export interface Board {
    id: number;
    summary: string;
    lists?: TaskList[];
}
