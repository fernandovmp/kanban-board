import { createContext, Dispatch, SetStateAction } from 'react';
import { Board, TaskList } from '../models';

interface IBoardContext {
    board?: Board;
    lists: TaskList[];
    setLists: Dispatch<SetStateAction<TaskList[]>>;
}

export const BoardContext = createContext<IBoardContext>({
    lists: [],
    setLists: () => {},
});
