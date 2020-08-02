import React, { useState } from 'react';
import addIcon from '../../../assets/add.svg';
import closeIcon from '../../../assets/close.svg';
import removeIcon from '../../../assets/remove.svg';
import { Modal } from '../../../components';
import {
    AddMemberWrapper,
    CloseButton,
    EmailInput,
    Header,
    IconButton,
    MemberCard,
    MemberItem,
    MembersList,
} from './styles';

interface IMembersModalProps {
    onClose(): void;
}

export const MembersModal: React.FC<IMembersModalProps> = ({ onClose }) => {
    const [userEmail, setUserEmail] = useState('');

    const handleAddMember = () => {};
    const handleRemoveMember = () => {};

    return (
        <Modal
            title={
                <Header>
                    <CloseButton
                        src={closeIcon}
                        alt="Close"
                        onClick={onClose}
                    />
                    <AddMemberWrapper>
                        <EmailInput
                            placeholder="User email"
                            value={userEmail}
                            onChange={(e) => setUserEmail(e.target.value)}
                        />
                        <IconButton
                            src={addIcon}
                            alt="Add"
                            onClick={handleAddMember}
                        />
                    </AddMemberWrapper>
                </Header>
            }
        >
            <MembersList>
                <MemberItem>
                    <MemberCard>Name (email@example.com)</MemberCard>
                    <IconButton
                        src={removeIcon}
                        alt="Remove"
                        onClick={handleRemoveMember}
                    />
                </MemberItem>
            </MembersList>
        </Modal>
    );
};
