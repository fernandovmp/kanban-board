import React, { useContext, useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import addIcon from '../../../assets/add.svg';
import { BoardContext } from '../../../contexts';
import { SummarizedTask, TaskList } from '../../../models';
import {
    apiPost,
    apiPut,
    isErrorResponse,
} from '../../../services/kanbanApiService';
import {
    Button,
    ButtonsWrapper,
    CancelButton,
    EditableListTitle,
    ListTitle,
    ListWrapper,
    NewTaskInput,
    SaveButton,
} from './styles';
import { TaskCard } from './TaskCard';
import { Tag, TaskWrapper } from './TaskCard/styles';

interface ITaskListProps {
    taskList: TaskList;
}

export const TaskListView: React.FC<ITaskListProps> = ({ taskList }) => {
    const [listTitle, setListTitle] = useState(taskList.title);
    const [tasks, setTasks] = useState<SummarizedTask[]>([]);
    const [isCreatingTask, setIsCreatingTask] = useState(false);
    const [newTaskSummary, setNewTaskSummary] = useState('');
    const boardContext = useContext(BoardContext);
    const history = useHistory();
    const { boardId } = useParams();

    useEffect(() => setTasks(taskList.tasks), [taskList]);

    const handleCreateTask = async () => {
        setIsCreatingTask(false);
        const summary = newTaskSummary.trim();
        setNewTaskSummary('');
        if (summary === '') return;

        const token = sessionStorage.getItem('jwtToken') ?? '';
        const response = await apiPost({
            uri: `v1/boards/${boardId}/tasks`,
            body: {
                summary,
                description: '',
                tagColor: 'FFEA31',
                assignedTo: [],
                list: taskList.id,
            },
            bearerToken: token,
        });
        if (isErrorResponse(response.data)) {
            if (response.data?.status === 401) {
                history.push('/login');
            }
            return;
        }
        const newTask = response.data!;
        const updateList = (lists: TaskList[]) => {
            const listWithContainsTheTask = lists.find(
                (list) => list.id === taskList.id
            );
            if (!listWithContainsTheTask) {
                return lists;
            }
            const filteredLists = lists.filter(
                (list) => list.id !== taskList.id
            );
            const updatedList = {
                ...listWithContainsTheTask,
                tasks: [...listWithContainsTheTask.tasks, newTask],
            };
            const finalLists = [...filteredLists, updatedList];
            return finalLists.sort((a, b) => a.id - b.id);
        };
        boardContext.setLists(updateList(boardContext.lists));
    };

    const handleEditListTitle = async (value: string) => {
        const newTitle = value.trim();
        if (newTitle === '') return;
        setListTitle(newTitle);
        const token = sessionStorage.getItem('jwtToken') ?? '';
        const response = await apiPut({
            uri: `v1/boards/${boardId}/lists/${taskList.id}`,
            body: {
                title: newTitle,
            },
            bearerToken: token,
        });
        if (response.data === undefined) return;
        if (isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
        }
    };

    return (
        <ListWrapper>
            <EditableListTitle
                onEndEdit={handleEditListTitle}
                initialInputValue={listTitle}
            >
                <ListTitle>{listTitle}</ListTitle>
            </EditableListTitle>
            {tasks.map((task) => (
                <TaskCard key={task.id} task={task} />
            ))}
            {isCreatingTask ? (
                <>
                    <TaskWrapper>
                        <Tag color="#FFEA31" />
                        <NewTaskInput
                            value={newTaskSummary}
                            onChange={(e) => setNewTaskSummary(e.target.value)}
                        />
                    </TaskWrapper>
                    <ButtonsWrapper>
                        <CancelButton onClick={() => setIsCreatingTask(false)}>
                            CANCEL
                        </CancelButton>
                        <SaveButton onClick={handleCreateTask}>SAVE</SaveButton>
                    </ButtonsWrapper>
                </>
            ) : (
                <Button onClick={() => setIsCreatingTask(true)}>
                    <img src={addIcon} alt="New Task" />
                    New Task
                </Button>
            )}
        </ListWrapper>
    );
};
