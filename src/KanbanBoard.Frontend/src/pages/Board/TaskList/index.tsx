import React, { useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import addIcon from '../../../assets/add.svg';
import { TaskList } from '../../../models';
import { apiPost, isErrorResponse } from '../../../services/kanbanApiService';
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
    const [tasks, setTasks] = useState(taskList.tasks);
    const [isCreatingTask, setIsCreatingTask] = useState(false);
    const [newTaskSummary, setNewTaskSummary] = useState('');
    const history = useHistory();
    const { boardId } = useParams();

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
        setTasks([...tasks, newTask]);
    };

    const handleEditListTitle = (value: string) => {
        if (value.trim() === '') return;
        setListTitle(value.trim());
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
