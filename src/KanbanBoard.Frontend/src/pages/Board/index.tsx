import React, { useEffect, useState } from 'react';
import { Route, useHistory, useParams } from 'react-router-dom';
import addIcon from '../../assets/add.svg';
import { Input } from '../../components';
import { Board, TaskList } from '../../models';
import {
    apiGet,
    apiPost,
    isErrorResponse,
} from '../../services/kanbanApiService';
import { BoardHeader } from './BoardHeader';
import {
    ButtonsWrapper,
    CancelButton,
    Main,
    NewListButton,
    SaveButton,
    TaskListsWrapper,
} from './styles';
import { TaskDetailModal } from './TaskDetailModal';
import { TaskListView } from './TaskList';
import { ListWrapper } from './TaskList/styles';

export const BoardPage: React.FC = () => {
    const [board, setBoard] = useState<Board>();
    const [boardLists, setBoardLists] = useState<TaskList[]>([]);
    const [newListTitle, setNewListTitle] = useState('');
    const [isCreatingList, setIsCreatingList] = useState(false);
    const history = useHistory();
    const { boardId } = useParams();

    useEffect(() => {
        const fetchBoard = async () => {
            const token = sessionStorage.getItem('jwtToken') ?? '';
            const response = await apiGet<Board>({
                uri: `v1/boards/${boardId}`,
                bearerToken: token,
            });
            if (!response.data) {
                return;
            }
            if (isErrorResponse(response.data)) {
                if (
                    response.data.status === 401 ||
                    response.data.status === 403
                ) {
                    history.push('/login');
                }
                return;
            }
            setBoard(response.data);
        };
        fetchBoard();
    }, [boardId, history]);

    useEffect(() => {
        setBoardLists(board?.lists ?? []);
    }, [board]);

    const handleCreateList = async () => {
        setIsCreatingList(false);
        const listTitle = newListTitle.trim();

        setNewListTitle('');

        if (listTitle === '') return;

        const token = sessionStorage.getItem('jwtToken');
        if (!token) {
            history.push('/login');
            return;
        }

        const response = await apiPost<TaskList>({
            uri: `v1/boards/${boardId}/lists`,
            body: {
                title: listTitle,
            },
            bearerToken: token,
        });
        if (isErrorResponse(response.data)) {
            return;
        }

        const taskList = response.data!;
        setBoardLists([...boardLists, taskList]);
    };

    return (
        <Main>
            <BoardHeader boardTitle={board?.title ?? ''} />
            <TaskListsWrapper>
                {boardLists.map((list) => (
                    <TaskListView key={list.id} taskList={list} />
                ))}
                {isCreatingList ? (
                    <ListWrapper>
                        <Input
                            autoFocus
                            value={newListTitle}
                            onChange={(e) => setNewListTitle(e.target.value)}
                        />
                        <ButtonsWrapper>
                            <CancelButton
                                onClick={() => setIsCreatingList(false)}
                            >
                                CANCEL
                            </CancelButton>
                            <SaveButton onClick={handleCreateList}>
                                SAVE
                            </SaveButton>
                        </ButtonsWrapper>
                    </ListWrapper>
                ) : (
                    <NewListButton onClick={() => setIsCreatingList(true)}>
                        <img src={addIcon} alt="New List" />
                        New list
                    </NewListButton>
                )}
            </TaskListsWrapper>
            <Route
                exact
                path="/board/:boardId/task/:taskId"
                component={TaskDetailModal}
            />
        </Main>
    );
};
