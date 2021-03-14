import React, { useState } from 'react';
import { useNavigation } from '@react-navigation/native';
import {
    Container,
    InputArea,
    CustumButton,
    CustumButtonText,
    SignMessageButton,
    SignMessageButtonText,
    SignMessageButtonTextBold
} from './styles';

import SignInput from '../../components/SignInput';
import CouveLogo from '../../assets/couve.svg';
import EmailIcon from '../../assets/email.svg';
import LockIcon from '../../assets/lock.svg';
import PersonIcon from '../../assets/person.svg';


export default () => {

    const navigation = useNavigation();

    const [nameField, setNameField] = useState('');
    const [nifField, setNifField] = useState('');
    const [emailField, setEmailField] = useState('');
    const [passwordField, setPasswordField] = useState('');

    const handleSignClick = () => {

    }

    const handleMessageButtonClick = () => {
        navigation.reset({
            routes: [{ name: 'SignIn' }]
        });
    }

    return (
        <Container>
            <CouveLogo width="100%" height="160" />

            <InputArea>
                <SignInput
                    IconSvg={PersonIcon}
                    placeholder="Introduza o nome"
                    value={nameField}
                    onChangeText={t => setNameField(t)}
                />

                <SignInput
                    IconSvg={PersonIcon}
                    placeholder="Introduza o NIF"
                    value={nifField}
                    onChangeText={t => setNifField(t)}
                />

                <SignInput
                    IconSvg={EmailIcon}
                    placeholder="Introduza o e-mail"
                    value={emailField}
                    onChangeText={t => setEmailField(t)}
                />

                <SignInput
                    IconSvg={LockIcon}
                    placeholder="Introduza a password"
                    value={passwordField}
                    onChangeText={t => setPasswordField(t)}
                    password={true}
                />

                <CustumButton onPress={handleSignClick}>
                    <CustumButtonText>CRIAR</CustumButtonText>
                </CustumButton>

            </InputArea>

            <SignMessageButton onPress={handleMessageButtonClick}>
                <SignMessageButtonText>Já possui uma conta?</SignMessageButtonText>
                <SignMessageButtonTextBold>Login</SignMessageButtonTextBold>
            </SignMessageButton>

        </Container>
    );
};