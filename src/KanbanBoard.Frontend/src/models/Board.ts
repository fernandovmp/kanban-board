import { TaskList } from './TaskList';

export interface Board {
    id: number;
    title: string;
    lists?: TaskList[];
}
