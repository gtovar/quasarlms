﻿
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dc="http://purl.org/dc/elements/1.1/" version="1.0">
        <xsl:output method="html"  />
    <xsl:template match="/">

<rule>
<root/>      
<HTML>       
<BODY bgcolor="white">
<center><hr width="70%"/><b> Академический календарь </b><hr width="70%"/><br/> 
<table width="90%" border="2">
        <tr>
          <td>Название</td>
          <td>1</td>
          <td>2</td>
          <td>3</td>
          <td>4</td>
          <td>5</td>
          <td>6</td>
          <td>7</td>
          <td>8</td>
          <td>9</td>
          <td>10</td>
          <td>11</td>
          <td>12</td>
          <td>13</td>
          <td>14</td>
          <td>15</td>
          <td>16</td>
          <td>17</td>
          <td>18</td>
          <td>19</td>
          <td>20</td>
          <td>21</td>
          <td>22</td>
          <td>23</td>
          <td>24</td>
          <td>25</td>
          <td>26</td>
          <td>27</td>
          <td>28</td>
          <td>29</td>
          <td>30</td>
          <td>31</td>
          <td>32</td>
          <td>33</td>
          <td>34</td>
          <td>35</td>
          <td>36</td>
          <td>37</td>
          <td>38</td>
          <td>39</td>
          <td>40</td>
          <td>41</td>
          <td>42</td>
          <td>43</td>
          <td>44</td>
          <td>45</td>
          <td>46</td>
          <td>47</td>
          <td>48</td>
          <td>49</td>
          <td>50</td>
          <td>51</td>
          <td>52</td>
        </tr>
    <xsl:apply-templates select="//ArrayOfString" />
<children/>  
</table></center>
</BODY>
</HTML>	
</rule>

    </xsl:template>

    <xsl:template match="ArrayOfString">
        <tr>
            <xsl:for-each select="string">
                <td>
                    <xsl:value-of select="." />
                </td>
            </xsl:for-each>
        </tr>
    </xsl:template>

    </xsl:stylesheet>
    
