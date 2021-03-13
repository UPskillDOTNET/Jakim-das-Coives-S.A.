import React, { useEffect, useState } from 'react';
import {FlatList, Text, AsyncStorage, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import {
    Container,
    InputArea,
    InpotArea,
    CustumButton,
    CustumButtonText,
    CustomButton,
    CustomButtonText,
    HeaderArea,
    HeaderTitle,
    Scroller
} from './styles';
import SignInput from '../../components/SignInput';
import Api from '../../Api';
import EmailIcon from '../../assets/lock.svg';

export default () => {
    const navigation = useNavigation();

    const [valor, setValor] = useState(null);
    const [nif, setNif] = useState('');


    //Logout button
    const handleLogoutClick = async () => {
        AsyncStorage.removeItem('token')
        AsyncStorage.removeItem('nif')
        navigation.reset({
            routes: [{name: 'SignIn'}]
        });
    }

    //consult Saldo
    const handleConsultClick = async () => {
        //navigation.navigate('Saldo');
            let json = await Api.getSaldo(nif);
                Alert.alert(
                    "O seu Saldo atual é:",
                    JSON.stringify(json),
                    [
                      {
                        text: "Cancel",
                        onPress: () => console.log("Cancel Pressed"),
                        style: "cancel"
                      },
                      { text: "OK", onPress: () => console.log("OK Pressed") }
                    ],
                    { cancelable: false }
                  );
            }

        
    


    //DEPOSITAR
    const handleDepositClick = async () => {
        if (valor != '') {
            let json = await Api.addValor(valor);
            Alert.alert(
                "Adicionou: ",
                JSON.stringify(json),
                [
                  {
                    text: "Cancel",
                    onPress: () => console.log("Cancel Pressed"),
                    style: "cancel"
                  },
                  { text: "OK", onPress: () => console.log("OK Pressed") }
                ],
                { cancelable: false }
              );

        } else {
            alert("Introduza um Valor!");  
        }
    }


    return (
        <Container>

            <Scroller>
                    <HeaderArea>
                        <HeaderTitle numberOfLines={2}>Faça a gestão da sua Carteira!</HeaderTitle>
                    </HeaderArea>
            </Scroller>

            <InpotArea>

                <SignInput
                    IconSvg={EmailIcon}
                    placeholder="Introduza o Valor"
                    value={valor}
                    onChangeText={t => setValor(t)}
                />

                <CustomButton onPress={handleDepositClick}>
                    <CustomButtonText>DEPOSITAR</CustomButtonText>
                </CustomButton>

                <CustomButton onPress={handleConsultClick}>
                    <CustomButtonText>CONSULTAR</CustomButtonText>
                </CustomButton>
            </InpotArea>

            <InputArea>

                <CustumButton onPress={handleLogoutClick}>
                    <CustumButtonText>LOGOUT</CustumButtonText>
                </CustumButton>

            </InputArea>
        </Container>
    );
}
