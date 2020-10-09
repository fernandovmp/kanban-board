import React, { useContext, useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import addIcon from '../../../assets/add.svg';
import optionsIcon from '../../../assets/more_vert.svg';
import { IconButton } from '../../../components';
import { BoardContext } from '../../../contexts';
import {
    addTaskToList,
    removeTaskFromList,
} from '../../../contexts/BoardContext/helpers';
import { SummarizedTask, TaskList } from '../../../models';
import {
    apiPatch,
    apiPost,
    apiPut,
    isErrorResponse,
} from '../../../services/kanbanApiService';
import { getJwtToken } from '../../../services/tokenService';
import { ListOptions } from './ListOptions';
import {
    Button,
    ButtonsWrapper,
    CancelButton,
    EditableListTitle,
    ListContent,
    ListHeader,
    ListTitle,
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
    const [showListOptions, setShowListOptions] = useState(false);
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
        boardContext.setLists(
            addTaskToList(boardContext.lists, taskList.id, newTask)
        );
    };

    const handleEditListTitle = async (value: string) => {
        const newTitle = value.trim();
        if (newTitle === '') return;
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
            return;
        }
        setListTitle(newTitle);
    };

    const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        event.dataTransfer.dropEffect = 'move';
    };

    const handleDrop = async (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        const data = event.dataTransfer.getData('task');
        const task = JSON.parse(data) as SummarizedTask;
        if (tasks.find((_task) => _task.id === task.id)) return;

        const listData = event.dataTransfer.getData('listId');
        const listId = Number(listData);
        let taskLists = removeTaskFromList(boardContext.lists, listId, task.id);
        taskLists = addTaskToList(taskLists, taskList.id, task);
        boardContext.setLists(taskLists);

        const token = getJwtToken();
        const response = await apiPatch({
            uri: `v1/boards/${boardId}/tasks/${task.id}`,
            body: {
                list: taskList.id,
            },
            bearerToken: token,
        });
        if (response.data && isErrorResponse(response.data)) {
            if (response.data.status === 401 || response.data.status === 403) {
                history.push('/login');
            }
        }
    };

    return (
        <ListContent onDragOver={handleDragOver} onDrop={handleDrop}>
            <ListHeader>
                <EditableListTitle
                    onEndEdit={handleEditListTitle}
                    initialInputValue={listTitle}
                >
                    <ListTitle>{listTitle}</ListTitle>
                </EditableListTitle>
                <IconButton
                    onClick={() => setShowListOptions(true)}
                    src={optionsIcon}
                />
                {showListOptions && (
                    <ListOptions
                        list={taskList}
                        onClose={() => setShowListOptions(false)}
                    />
                )}
            </ListHeader>
            {tasks.map((task) => (
                <TaskCard key={task.id} task={task} listId={taskList.id} />
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
        </ListContent>
    );
};
