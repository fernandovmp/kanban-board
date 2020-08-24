import React, { useEffect, useState } from 'react';
import assignmentIcon from '../../../../assets/assignment.svg';
import deleteIcon from '../../../../assets/delete_outline.svg';
import { Task, User } from '../../../../models';
import { EditAssignments } from '../EditAssignments';
import {
    AssignedMemberName,
    AssignmentSectionTitle,
    Button,
    Color,
    SectionTitle,
    SidePanelWrapper,
    TagColorInput,
    TagColors,
} from './styles';

const colors = [
    '#FFEA31',
    '#FF3131',
    '#0069E4',
    '#00E409',
    '#8E00E4',
    '#9F9F9F',
];

interface ISidePanelProps {
    task?: Task;
}

export const SidePanel: React.FC<ISidePanelProps> = ({ task }) => {
    const [selectedTagColor, setSelectedTagColor] = useState('');
    const [showAssignmentsModal, setShowAssignmentsModal] = useState(false);
    const [taskAssignments, setTaskAssignments] = useState<User[]>([]);

    useEffect(() => setSelectedTagColor(`#${task?.tagColor}`), [task]);
    useEffect(() => setTaskAssignments(task?.assignments ?? []), [task]);

    return (
        <>
            <SidePanelWrapper>
                <SectionTitle>Tag</SectionTitle>
                <TagColorInput
                    value={selectedTagColor}
                    onChange={(e) => setSelectedTagColor(e.target.value)}
                />
                <TagColors>
                    {colors.map((color, index) => (
                        <Color
                            key={index}
                            color={color}
                            onClick={() => setSelectedTagColor(color)}
                        />
                    ))}
                </TagColors>
                <Button>
                    <img src={deleteIcon} alt="Delete" /> DELETE TASK
                </Button>
                <AssignmentSectionTitle>
                    <img src={assignmentIcon} alt="Assignments" /> Assignments
                </AssignmentSectionTitle>
                <Button onClick={() => setShowAssignmentsModal(true)}>
                    EDIT ASSIGNMENTS
                </Button>
                {taskAssignments.map((assignedMember, index) => (
                    <AssignedMemberName key={index}>
                        {assignedMember.name}
                    </AssignedMemberName>
                ))}
            </SidePanelWrapper>
            {showAssignmentsModal && task && (
                <EditAssignments
                    taskAssignments={taskAssignments}
                    onAssignmentsChange={setTaskAssignments}
                    onClose={() => setShowAssignmentsModal(false)}
                />
            )}
        </>
    );
};
