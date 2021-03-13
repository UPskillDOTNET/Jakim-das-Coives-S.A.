import React, { useState, useContext } from 'react';
import { useNavigation } from '@react-navigation/native';
import {UserContext} from '../../contexts/UserContext';
import { AsyncStorage } from 'react-native';

import {
    Container,
    InputArea,
    CustumButton,
    CustumButtonText,
    SignMessageButton,
    SignMessageButtonText,
    SignMessageButtonTextBold
} from './styles';

import Api from '../../Api';

import SignInput from '../../components/SignInput';
import CouveLogo from '../../assets/couve.svg';
import EmailIcon from '../../assets/email.svg';
import LockIcon from '../../assets/lock.svg';


export default () => {
    const {dispatch: userDispatch} = useContext(UserContext);
    const navigation = useNavigation();

    const [emailField, setEmailField] = useState('');
    const [passwordField, setPasswordField] = useState('');

    const handleSignClick = async () => {
        if (emailField != '' && passwordField != '') {
            let json = await Api.signIn(emailField, passwordField);
            if (json.token, json.nif) {
                //alert('It funfates!');
                await AsyncStorage.setItem('token', json.token);
                await AsyncStorage.setItem('nif', json.nif);

                navigation.reset({
                    routes:[{name: 'MainTab'}]
                });
            } else {
                alert('E-mail e/ou password inválido!');
            }

        } else {
            alert("Preencha os campos em falta!");
        }
    }

    const handleMessageButtonClick = () => {
        navigation.reset({
            routes: [{name:'SignUp'}]
        });
    }



    return (
        <Container>
            <CouveLogo width="100%" height="160" />

            <InputArea>
                
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
                    <CustumButtonText>LOGIN</CustumButtonText>
                </CustumButton>

            </InputArea>

            <SignMessageButton onPress={handleMessageButtonClick} >
                <SignMessageButtonText>Não possui conta?</SignMessageButtonText>
                <SignMessageButtonTextBold>Crie Conta</SignMessageButtonTextBold>
            </SignMessageButton>

        </Container>
    );
};