'****************************************************
'* Teknovation Ltd 
'* Copyright 2012
'*
'****************************************************


Imports System
Imports System.Collections
Imports System.Xml
Imports System.IO
Imports System.Text

Namespace Flash.External
    ' <summary> 
    ' Provides methods to convert the XML communication format to .NET classes. Typically 
    ' this class will not be used directly; it supports the operations of the 
    ' ExternalInterfaceProxy class. 
    ' </summary> 
    Public Class ExternalInterfaceSerializer
#Region "Constructor"

        Private Sub New()
        End Sub

#End Region

#Region "Public Methods"

        ' <summary> 
        ' Encodes a function call to be sent to Flash. 
        ' </summary> 
        ' <param name="functionName">The name of the function to call.</param> 
        ' <param name="arguments">Zero or more parameters to pass in to the ActonScript function.</param> 
        ' <returns>The XML string representation of the function call to pass to Flash</returns> 
        Public Shared Function EncodeInvoke(ByVal functionName As String, ByVal arguments As Object()) As String
            Dim sb As New StringBuilder()

            Dim writer As New XmlTextWriter(New StringWriter(sb))

            ' <invoke name="functionName" returntype="xml"> 
            writer.WriteStartElement("invoke")
            writer.WriteAttributeString("name", functionName)
            writer.WriteAttributeString("returntype", "xml")

            If arguments IsNot Nothing AndAlso arguments.Length > 0 Then
                ' <arguments> 
                writer.WriteStartElement("arguments")

                ' individual arguments 
                For Each value As Object In arguments
                    WriteElement(writer, value)
                Next

                ' </arguments> 
                writer.WriteEndElement()
            End If

            ' </invoke> 
            writer.WriteEndElement()

            writer.Flush()
            writer.Close()

            Return sb.ToString()
        End Function

        ' <summary> 
        ' Encodes a value to send to Flash as the result of a function call from Flash. 
        ' </summary> 
        ' <param name="value">The value to encode.</param> 
        ' <returns>The XML string representation of the value.</returns> 
        Public Shared Function EncodeResult(ByVal value As Object) As String
            Dim sb As New StringBuilder()

            Dim writer As New XmlTextWriter(New StringWriter(sb))

            WriteElement(writer, value)

            writer.Flush()
            writer.Close()

            Return sb.ToString()
        End Function


        ' <summary> 
        ' Decodes a function call from Flash. 
        ' </summary> 
        ' <param name="xml">The XML string representing the function call.</param> 
        ' <returns>An ExternalInterfaceCall object representing the function call.</returns> 
        Public Shared Function DecodeInvoke(ByVal xml As String) As ExternalInterfaceCall
            Dim reader As New XmlTextReader(xml, XmlNodeType.Document, Nothing)

            reader.Read()

            Dim functionName As String = reader.GetAttribute("name")
            Dim result As New ExternalInterfaceCall(functionName)

            reader.ReadStartElement("invoke")
            reader.ReadStartElement("arguments")

            While reader.NodeType <> XmlNodeType.EndElement AndAlso reader.Name <> "arguments"
                result.AddArgument(ReadElement(reader))
            End While

            reader.ReadEndElement()
            reader.ReadEndElement()

            Return result
        End Function


        ' <summary> 
        ' Decodes the result of a function call to Flash 
        ' </summary> 
        ' <param name="xml">The XML string representing the result.</param> 
        ' <returns>A <see cref="System.Object"/> containing the result</returns> 
        Public Shared Function DecodeResult(ByVal xml As String) As Object
            Dim reader As New XmlTextReader(xml, XmlNodeType.Document, Nothing)
            reader.Read()
            Return ReadElement(reader)
        End Function

#End Region

#Region "Writers"

        Private Shared Sub WriteElement(ByVal writer As XmlTextWriter, ByVal value As Object)
            If value Is Nothing Then
                writer.WriteStartElement("null")
                writer.WriteEndElement()
            ElseIf TypeOf value Is String Then
                writer.WriteStartElement("string")
                writer.WriteString(value.ToString())
                writer.WriteEndElement()
            ElseIf TypeOf value Is Boolean Then
                writer.WriteStartElement(IIf(CBool(value), "true", "false"))
                writer.WriteEndElement()
            ElseIf TypeOf value Is Single OrElse TypeOf value Is Double OrElse TypeOf value Is Integer OrElse TypeOf value Is UInteger Then
                writer.WriteStartElement("number")
                writer.WriteString(value.ToString())
                writer.WriteEndElement()
            ElseIf TypeOf value Is ArrayList Then
                WriteArray(writer, DirectCast(value, ArrayList))
            ElseIf TypeOf value Is Hashtable Then
                WriteObject(writer, DirectCast(value, Hashtable))
            Else
                ' null is the default when ActionScript can't serialize an object 
                writer.WriteStartElement("null")
                writer.WriteEndElement()
            End If
        End Sub


        Private Shared Sub WriteArray(ByVal writer As XmlTextWriter, ByVal array As ArrayList)
            writer.WriteStartElement("array")

            Dim len As Integer = array.Count
            For i As Integer = 0 To len - 1

                writer.WriteStartElement("property")
                writer.WriteAttributeString("id", i.ToString())
                WriteElement(writer, array(i))
                writer.WriteEndElement()
            Next

            writer.WriteEndElement()
        End Sub


        Private Shared Sub WriteObject(ByVal writer As XmlTextWriter, ByVal table As Hashtable)
            writer.WriteStartElement("object")

            For Each entry As DictionaryEntry In table
                writer.WriteStartElement("property")
                writer.WriteAttributeString("id", entry.Key.ToString())
                WriteElement(writer, entry.Value)
                writer.WriteEndElement()
            Next

            writer.WriteEndElement()
        End Sub

#End Region

#Region "Readers"

        Private Shared Function ReadElement(ByVal reader As XmlTextReader) As Object
            If reader.NodeType <> XmlNodeType.Element Then
                Throw New XmlException()
            End If

            If reader.Name = "true" Then
                reader.Read()
                Return True
            End If

            If reader.Name = "false" Then
                reader.Read()
                Return False
            End If

            If reader.Name = "null" OrElse reader.Name = "undefined" Then
                reader.Read()
                Return Nothing
            End If

            If reader.IsStartElement("number") Then
                reader.ReadStartElement("number")
                Dim value As Double = [Double].Parse(reader.Value)
                reader.Read()
                reader.ReadEndElement()
                Return value
            End If

            If reader.IsStartElement("string") Then
                reader.ReadStartElement("string")
                Dim value As String = reader.Value
                reader.Read()
                reader.ReadEndElement()
                Return value
            End If

            If reader.IsStartElement("array") Then
                reader.ReadStartElement("array")
                Dim value As ArrayList = ReadArray(reader)
                reader.ReadEndElement()
                Return value
            End If

            If reader.IsStartElement("object") Then
                reader.ReadStartElement("object")
                Dim value As Hashtable = ReadObject(reader)
                reader.ReadEndElement()
                Return value
            End If
            Throw New XmlException()
        End Function


        Private Shared Function ReadArray(ByVal reader As XmlTextReader) As ArrayList
            Dim result As New ArrayList()

            While reader.NodeType <> XmlNodeType.EndElement AndAlso reader.Name <> "array"
                Dim id As Integer = Integer.Parse(reader.GetAttribute("id"))
                reader.ReadStartElement("property")
                result.Add(ReadElement(reader))
                reader.ReadEndElement()
            End While

            Return result
        End Function


        Private Shared Function ReadObject(ByVal reader As XmlTextReader) As Hashtable
            Dim result As New Hashtable()

            While reader.NodeType <> XmlNodeType.EndElement AndAlso reader.Name <> "object"
                Dim id As String = reader.GetAttribute("id")
                reader.ReadStartElement("property")
                result.Add(id, ReadElement(reader))
                reader.ReadEndElement()
            End While

            Return result
        End Function

#End Region
    End Class
End Namespace